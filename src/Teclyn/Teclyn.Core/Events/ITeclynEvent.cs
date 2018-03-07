using Teclyn.Core.Domains;

namespace Teclyn.Core.Events
{
    public interface ITeclynEvent
    {
        string AggregateId { get; set; }
    }

    public interface ITeclynEvent<out TAggregate> : ITeclynEvent where TAggregate : IAggregate
    {

    }
}