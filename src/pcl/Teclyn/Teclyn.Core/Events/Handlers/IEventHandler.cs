using Teclyn.Core.Domains;

namespace Teclyn.Core.Events.Handlers
{
    public interface IEventHandler
    {
        
    }

    public interface IEventHandler<in TEvent> : IEventHandler where TEvent : ITeclynEvent
    {
        void Handle(IEventInformation<TEvent> @event);
    }

    public interface IEventHandler<in TAggregate, in TEvent> : IEventHandler where TAggregate : class, IAggregate where TEvent : ITeclynEvent<TAggregate>
    {
        void Handle(TAggregate aggregate, IEventInformation<TEvent> @event);
    }
}