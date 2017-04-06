using System.Threading.Tasks;
using Teclyn.Core.Domains;

namespace Teclyn.Core.Events.Handlers
{
    public interface IEventHandler
    {
        
    }

    public interface IEventHandler<in TEvent> : IEventHandler where TEvent : ITeclynEvent
    {
        Task Handle(TEvent @event);
    }

    public interface IEventHandler<in TAggregate, in TEvent> : IEventHandler where TAggregate : class, IAggregate where TEvent : ITeclynEvent<TAggregate>
    {
        Task Handle(TAggregate aggregate, TEvent @event);
    }
}