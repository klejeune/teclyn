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

        public EventService(ITeclynContext teclynContext, Time time, RepositoryService repositoryService, EventHandlerService eventHandlerService)
        {
            this.teclynContext = teclynContext;
            this.time = time;
            this.repositoryService = repositoryService;
            this.eventHandlerService = eventHandlerService;
        }

        public TAggregate Raise<TAggregate>(ICreationEvent<TAggregate> @event) where TAggregate : IAggregate
        {
            var eventInformation = this.BuildEventInformation(@event);
            var aggregate = this.BuildAggregate<TAggregate>();

            @event.Apply(aggregate, eventInformation);

            this.LaunchEventHandlers(aggregate, @event, eventInformation);

            return aggregate;
        }

        public TAggregate Raise<TAggregate>(IModificationEvent<TAggregate> @event) where TAggregate : class, IAggregate
        {
            var eventInformation = this.BuildEventInformation(@event);
            var aggregate = this.repositoryService.Get<TAggregate>().GetById(@event.AggregateId);

            @event.Apply(aggregate, eventInformation);

            this.LaunchEventHandlers(aggregate, @event, eventInformation);

            return aggregate;
        }

        public TAggregate Raise<TAggregate>(ISuppressionEvent<TAggregate> @event) where TAggregate : class, IAggregate
        {
            var eventInformation = this.BuildEventInformation(@event);
            var aggregate = this.repositoryService.Get<TAggregate>().GetById(@event.AggregateId);

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
            eventInformation.User = this.teclynContext.CurrentUser;
            eventInformation.Date = this.time.Now;
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