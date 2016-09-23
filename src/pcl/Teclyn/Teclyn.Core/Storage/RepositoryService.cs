using System;
using System.Collections.Generic;
using Teclyn.Core.Api;
using Teclyn.Core.Domains;
using Teclyn.Core.Events.Handlers;
using Teclyn.Core.Ioc;
using Teclyn.Core.Storage.EventHandlers;

namespace Teclyn.Core.Storage
{
    public class RepositoryService
    {
        private TeclynApi teclyn;
        private IDictionary<Type, AggregateInfo> aggregates = new Dictionary<Type, AggregateInfo>();
        private IIocContainer iocContainer;
        private EventHandlerService eventHandlerService;

        public RepositoryService(TeclynApi teclyn, IIocContainer iocContainer, EventHandlerService eventHandlerService)
        {
            this.teclyn = teclyn;
            this.iocContainer = iocContainer;
            this.eventHandlerService = eventHandlerService;
        }

        public IRepository<T> Get<T>() where T : class, IAggregate
        {
            return this.teclyn.Get<IRepository<T>>();
        }

        public void Register(Type aggregateType, Type implementationType, string collectionName, Type accessControllerType, Type defaultFilterType)
        {
            this.aggregates[aggregateType] = new AggregateInfo(aggregateType, implementationType, collectionName, accessControllerType, defaultFilterType);

            this.iocContainer.Register(typeof(IRepository<>).MakeGenericType(aggregateType), typeof(Repository<>).MakeGenericType(aggregateType));
            
            this.eventHandlerService.RegisterEventHandler(typeof(ModificationPersistenceEventHandler<>).MakeGenericType(aggregateType));
            this.eventHandlerService.RegisterEventHandler(typeof(SuppressionPersistenceEventHandler<>).MakeGenericType(aggregateType));
        }

        public AggregateInfo GetInfo<T>()
        {
            return this.aggregates[typeof(T)];
        }

        public IEnumerable<AggregateInfo> Aggregates
        {
            get { return this.aggregates.Values; }
        }
    }
}