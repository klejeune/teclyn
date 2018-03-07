using System;
using System.Linq;
using System.Threading.Tasks;
using Teclyn.Core.Api;
using Teclyn.Core.Domains;
using Teclyn.Core.Events.Handlers;
using Teclyn.Core.Ioc;
using Teclyn.Core.Security.Context;
using Teclyn.Core.Services;
using Teclyn.Core.Storage;
using Teclyn.Core.Tools;

namespace Teclyn.Core.Events
{
    [ServiceImplementation]
    public class EventService : IEventService
    {
        private readonly IDependencyResolver _dependencyResolver;
        private readonly ITeclynApi _teclynApi;
        private readonly ITeclynContext _teclynContext;
        private readonly ITimeService _timeService;
        private readonly IEventHandlerService _eventHandlerService;
        
        public IRepository<IEventInformation> EventInformationRepository { get; set; }
        
        public IdGenerator IdGenerator { get; set; }

        public EventService(IDependencyResolver dependencyResolver, ITeclynApi teclynApi, ITeclynContext teclynContext, ITimeService timeService, IEventHandlerService eventHandlerService)
        {
            this._dependencyResolver = dependencyResolver;
            this._teclynApi = teclynApi;
            this._teclynContext = teclynContext;
            this._timeService = timeService;
            this._eventHandlerService = eventHandlerService;
        }

        public async Task<TAggregate> Raise<TAggregate>(IEvent<TAggregate> @event) where TAggregate : class, IAggregate
        {
            var eventInformation = this.BuildEventInformation(@event);
            await this.EventInformationRepository.Create(eventInformation);
            var aggregate = await this._dependencyResolver.Get<IRepository<TAggregate>>().GetByIdOrNull(@event.AggregateId);

            if (aggregate == null)
            {
                aggregate = this.BuildAggregate<TAggregate>();
            }
            
            @event.Apply(aggregate);

            await this.LaunchEventHandlers(aggregate, @event);

            return aggregate;
        }

        public async Task<TAggregate> Raise<TAggregate>(ISuppressionEvent<TAggregate> @event) where TAggregate : class, IAggregate
        {
            var eventInformation = this.BuildEventInformation(@event);
            await this.EventInformationRepository.Create(eventInformation);
            var aggregate = await this._dependencyResolver.Get<IRepository<TAggregate>>().GetById(@event.AggregateId);

            await this.LaunchEventHandlers(aggregate, @event);

            return aggregate;
        }

        private IEventInformation BuildEventInformation(ITeclynEvent @event)
        {
            var eventInformation = new EventInformation();
            eventInformation.Id = this.IdGenerator.GenerateId();
            eventInformation.User = this._teclynContext.CurrentUser;
            eventInformation.Date = this._timeService.Now();
            eventInformation.EventType = @event.GetType().ToString();
            eventInformation.Event = @event;

            return eventInformation;
        }

        private async Task LaunchEventHandlers(IAggregate aggregate, ITeclynEvent @event)
        {
            var eventTypeAncestors = @event.GetType().GetAllAncestorsAndInterfaces();

            var handlers = eventTypeAncestors
                .SelectMany(ancestorType => this._eventHandlerService.GetEventHandlers(ancestorType))
                .Select(handler => handler.GetHandleAction(aggregate, @event));

            await Task.WhenAll(handlers.Select(t => t()));
        }

        private TAggregate BuildAggregate<TAggregate>() where TAggregate : IAggregate
        {
            var aggregateInfo = this._teclynApi.GetAggregate<TAggregate>();

            var imlementationType = aggregateInfo.ImplementationType;

            return (TAggregate) Activator.CreateInstance(imlementationType);
        }
    }
}