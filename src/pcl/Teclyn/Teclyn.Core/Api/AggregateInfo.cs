using System;

namespace Teclyn.Core.Api
{
    public class AggregateInfo
    {
        public Type AggregateType { get; }
        public Type ImplementationType { get; }

        public AggregateInfo(Type aggregateType, Type implementationType)
        {
            this.AggregateType = aggregateType;
            this.ImplementationType = implementationType;
        }
    }
}