using System.Threading.Tasks;
using Teclyn.Core.Domains;
using Teclyn.Core.Events;
using Teclyn.Core.Events.Handlers;
using Teclyn.Core.Ioc;

namespace Teclyn.Core.Storage.EventHandlers
{
    public class SuppressionPersistenceEventHandler<TAggregate> : IEventHandler<TAggregate, ISuppressionEvent<TAggregate>> where TAggregate : class, IAggregate
    {
        public IRepository<TAggregate> Repository { get; set; }
        
        public async Task Handle(TAggregate aggregate, ISuppressionEvent<TAggregate> @event)
        {
            await this.Repository.Delete(aggregate);
        }
    }
}