using System;

namespace Teclyn.Core.Api
{
    public class AggregateInfo
    {
        public Type AggregateType { get; }
        public Type ImplementationType { get; }
        public string CollectionName { get; }

        public AggregateInfo(Type aggregateType, Type implementationType, string collectionName)
        {
            this.AggregateType = aggregateType;
            this.ImplementationType = implementationType;
            this.CollectionName = collectionName;
        }
    }
}