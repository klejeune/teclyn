using System;
using System.Linq;
using Teclyn.Core.Domains;
using Teclyn.Core.Dummies;
using Teclyn.Core.Events.Handlers;
using Teclyn.Core.Ioc;
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

        [Inject]
        public IRepository<IEventInformation> EventInformationRepository { get; set; }

        [Inject]
        public IdGenerator IdGenerator { get; set; }

        public EventService(ITeclynContext teclynContext, Time time, RepositoryService repositoryService, EventHandlerService eventHandlerService)
        {
            this.teclynContext = teclynContext;
            this.time = time;
            this.repositoryService = repositoryService;
            this.eventHandlerService = eventHandlerService;
        }

        public TAggregate Raise<TAggregate>(IEvent<TAggregate> @event) where TAggregate : class, IAggregate
        {
            var eventInformation = this.BuildEventInformation(@event);
            this.EventInformationRepository.Create(eventInformation);
            var aggregate = this.repositoryService.Get<TAggregate>().GetByIdOrNull(@event.AggregateId);

            if (aggregate == null)
            {
                aggregate = this.BuildAggregate<TAggregate>();
            }
            
            @event.Apply(aggregate, eventInformation);

            this.LaunchEventHandlers(aggregate, @event, eventInformation);

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

        private void LaunchEventHandlers(IAggregate aggregate, ITeclynEvent @event, IEventInformation eventInformation)
        {
            var eventTypeAncestors = @event.GetType().GetAllAncestorsAndInterfaces();

            var handlers = eventTypeAncestors
                .SelectMany(ancestorType => this.eventHandlerService.GetEventHandlers(ancestorType))
                .Select(handler => handler.GetHandleAction(aggregate, @event, eventInformation));

            foreach (var handler in handlers)
            {
                handler();
            }
        }

        private TAggregate BuildAggregate<TAggregate>() where TAggregate : IAggregate
        {
            var aggregateInfo = this.repositoryService.GetInfo<TAggregate>();

            var imlementationType = aggregateInfo.ImplementationType;

            return (TAggregate) Activator.CreateInstance(imlementationType);
        }
    }
}