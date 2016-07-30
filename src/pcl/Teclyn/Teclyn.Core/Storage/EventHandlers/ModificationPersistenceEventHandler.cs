using Teclyn.Core.Domains;
using Teclyn.Core.Events;
using Teclyn.Core.Events.Handlers;
using Teclyn.Core.Ioc;

namespace Teclyn.Core.Storage.EventHandlers
{
    public class ModificationPersistenceEventHandler<TAggregate> : IEventHandler<TAggregate, IModificationEvent<TAggregate>> where TAggregate : class, IAggregate
    {
        [Inject]
        public IRepository<TAggregate> Repository { get; set; }
        
        public void Handle(TAggregate aggregate, IEventInformation<IModificationEvent<TAggregate>> @event)
        {
            this.Repository.Save(aggregate);
        }
    }
}