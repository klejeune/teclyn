using System;
using System.Linq;
using System.Threading.Tasks;
using Teclyn.Core.Domains;
using Teclyn.Core.Dummies;
using Teclyn.Core.Events.Handlers;
using Teclyn.Core.Ioc;
using Teclyn.Core.Metadata;
using Teclyn.Core.Security;
using Teclyn.Core.Security.Context;
using Teclyn.Core.Services;
using Teclyn.Core.Storage;
using Teclyn.Core.Tools;

namespace Teclyn.Core.Events
{
    public class EventService
    {
        private ITeclynContext teclynContext;
        private Time time;
        private RepositoryService repositoryService;
        private EventHandlerService eventHandlerService;
        private MetadataRepository metadataRepository;

        [Inject]
        public IRepository<IEventInformation> EventInformationRepository { get; set; }

        [Inject]
        public IdGenerator IdGenerator { get; set; }

        public EventService(ITeclynContext teclynContext, Time time, RepositoryService repositoryService, EventHandlerService eventHandlerService, MetadataRepository metadataRepository)
        {
            this.teclynContext = teclynContext;
            this.time = time;
            this.repositoryService = repositoryService;
            this.eventHandlerService = eventHandlerService;
            this.metadataRepository = metadataRepository;
        }

        public async Task<TAggregate> Raise<TAggregate>(IEvent<TAggregate> @event) where TAggregate : class, IAggregate
        {
            var eventInformation = this.BuildEventInformation(@event);
            await this.EventInformationRepository.Create(eventInformation);
            var aggregate = await this.repositoryService.Get<TAggregate>().GetByIdOrNull(@event.AggregateId);

            if (aggregate == null)
            {
                aggregate = this.BuildAggregate<TAggregate>();
            }
            
            @event.Apply(aggregate, eventInformation);

            await this.LaunchEventHandlers(aggregate, @event, eventInformation);

            return aggregate;
        }

        public async Task<TAggregate> Raise<TAggregate>(ISuppressionEvent<TAggregate> @event) where TAggregate : class, IAggregate
        {
            var eventInformation = this.BuildEventInformation(@event);
            var aggregate = await this.repositoryService.Get<TAggregate>().GetById(@event.AggregateId);

            await this.LaunchEventHandlers(aggregate, @event, eventInformation);

            return aggregate;
        }

        private IEventInformation BuildEventInformation(ITeclynEvent @event)
        {
            var eventType = @event.GetType();
            var buildTypedEventInformationMethod = ReflectionTools.Instance<EventService>
                .Method(eventService => eventService.BuildTypedEventInformation<DummyCreationEvent>(null))
                .GetGenericMethodDefinition()
                .MakeGenericMethod(eventType);

            var result = buildTypedEventInformationMethod.Invoke(this, new object[] {@event});

            return (IEventInformation) result;
        }

        private EventInformation<TEvent> BuildTypedEventInformation<TEvent>(TEvent @event) where TEvent : ITeclynEvent
        {
            var eventInformation = new EventInformation<TEvent>();
            eventInformation.Id = this.IdGenerator.GenerateId();
            eventInformation.User = this.teclynContext.CurrentUser;
            eventInformation.Date = this.time.Now;
            eventInformation.EventType = @event.GetType().ToString();
            eventInformation.Event = @event;

            return eventInformation;
        }

        private async Task LaunchEventHandlers(IAggregate aggregate, ITeclynEvent @event, IEventInformation eventInformation)
        {
            var eventTypeAncestors = @event.GetType().GetAllAncestorsAndInterfaces();

            var handlers = eventTypeAncestors
                .SelectMany(ancestorType => this.eventHandlerService.GetEventHandlers(ancestorType))
                .Select(handler => handler.GetHandleAction(aggregate, @event, eventInformation));

            await Task.WhenAll(handlers.Select(t => t()));
        }

        private TAggregate BuildAggregate<TAggregate>() where TAggregate : IAggregate
        {
            var aggregateInfo = this.repositoryService.GetInfo<TAggregate>();

            var imlementationType = aggregateInfo.ImplementationType;

            return (TAggregate) Activator.CreateInstance(imlementationType);
        }

        public void RegisterEvent(Type eventType)
        {
            this.metadataRepository.RegisterEvent(new EventInfo(eventType.Name.ToLowerInvariant(), eventType.Name, eventType));
        }
    }
}