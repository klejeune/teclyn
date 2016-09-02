using Teclyn.Core.Domains;

namespace Teclyn.Core.Events
{
    public interface ISuppressionEvent<TAggregate> : IEvent<TAggregate> where TAggregate : IAggregate
    {
       
    }
}