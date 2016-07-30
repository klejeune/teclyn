using Teclyn.Core.Domains;

namespace Teclyn.Core.Events
{
    public interface ISuppressionEvent<out TAggregate> : ITeclynEvent<TAggregate> where TAggregate : IAggregate
    {
       
    }
}