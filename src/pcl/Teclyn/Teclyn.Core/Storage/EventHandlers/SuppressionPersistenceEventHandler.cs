using Teclyn.Core.Domains;
using Teclyn.Core.Events;
using Teclyn.Core.Events.Handlers;
using Teclyn.Core.Ioc;

namespace Teclyn.Core.Storage.EventHandlers
{
    public class SuppressionPersistenceEventHandler<TAggregate> : IEventHandler<TAggregate, ISuppressionEvent<TAggregate>> where TAggregate : class, IAggregate
    {
        [Inject]
        public IRepository<TAggregate> Repository { get; set; }
        
        public void Handle(TAggregate aggregate, IEventInformation<ISuppressionEvent<TAggregate>> @event)
        {
            this.Repository.Delete(aggregate);
        }
    }
}