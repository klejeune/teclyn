using System;
using System.Threading.Tasks;
using Teclyn.Core.Domains;
using Teclyn.Core.Events;
using Teclyn.Core.Events.Handlers;
using Teclyn.Core.Ioc;

namespace Teclyn.Core.Storage.EventHandlers
{
    public class ModificationPersistenceEventHandler<TAggregate> : IEventHandler<TAggregate, IEvent<TAggregate>> where TAggregate : class, IAggregate
    {
        [Inject]
        public IRepository<TAggregate> Repository { get; set; }
        
        public async Task Handle(TAggregate aggregate, IEvent<TAggregate> @event)
        {
            if (await this.Repository.Exists(aggregate.Id))
            {
                await this.Repository.Save(aggregate);
            }
            else
            {
                await this.Repository.Create(aggregate);
            }
        }
    }
}