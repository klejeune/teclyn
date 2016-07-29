using Teclyn.Core.Domains;

namespace Teclyn.Core.Events
{
    public interface IModificationEvent<TAggregate> : ITeclynEvent<TAggregate> where TAggregate : IAggregate
    {
        void Apply(TAggregate aggregate, IEventInformation information);
    }
}