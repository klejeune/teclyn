using System.Threading.Tasks;
using Teclyn.Core.Domains;
using Teclyn.Core.Storage;

namespace Teclyn.Core.Events
{
    public static class EventExtensions
    {
        public static async Task<TAggregate> GetAggregate<TAggregate>(this IEvent<TAggregate> @event, TeclynApi teclyn) where TAggregate : class, IAggregate
        {
            return await teclyn.Get<IRepository<TAggregate>>().GetById(@event.AggregateId);
        }
    }
}