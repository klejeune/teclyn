using System.Threading.Tasks;
using Teclyn.Core.Domains;

namespace Teclyn.Core.Events
{
    [Service]
    public interface IEventService
    {
        Task<TAggregate> Raise<TAggregate>(IEvent<TAggregate> @event) where TAggregate : class, IAggregate;
        Task<TAggregate> Raise<TAggregate>(ISuppressionEvent<TAggregate> @event) where TAggregate : class, IAggregate;
    }
}