using Teclyn.Core.Domains;

namespace Teclyn.Core.Events
{
    public interface ICreationEvent<out TAggregate> : ITeclynEvent<TAggregate> where TAggregate : IAggregate
    {
        TAggregate Apply(IEventInformation information);
    }
}