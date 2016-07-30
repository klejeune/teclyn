using System;
using Teclyn.Core.Domains;
using Teclyn.Core.Events;
using Teclyn.Core.Events.Handlers;
using Teclyn.Core.Ioc;

namespace Teclyn.Core.Storage.EventHandlers
{
    public class CreationPersistenceEventHandler<TAggregate> : IEventHandler<TAggregate, ICreationEvent<TAggregate>> where TAggregate : class, IAggregate
    {
        [Inject]
        public IRepository<TAggregate> Repository { get; set; }
        
        public void Handle(TAggregate aggregate, IEventInformation<ICreationEvent<TAggregate>> @event)
        {
            this.Repository.Create(aggregate);
        }
    }
}