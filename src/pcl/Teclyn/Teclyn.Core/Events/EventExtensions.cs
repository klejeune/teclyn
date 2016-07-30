using Teclyn.Core.Domains;
using Teclyn.Core.Storage;

namespace Teclyn.Core.Events
{
    public static class EventExtensions
    {
        public static TAggregate GetAggregate<TAggregate>(this ITeclynEvent<TAggregate> @event, TeclynApi teclyn) where TAggregate : class, IAggregate
        {
            return teclyn.Get<IRepository<TAggregate>>().GetById(@event.AggregateId);
        }
    }
}