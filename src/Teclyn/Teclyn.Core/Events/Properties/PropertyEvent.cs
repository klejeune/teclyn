using Teclyn.Core.Domains;

namespace Teclyn.Core.Events.Properties
{
    public abstract class PropertyEvent<TAggregate, TProperty> : IPropertyEvent<TAggregate, TProperty> where TAggregate : IAggregate
    {
        public abstract void Apply(TAggregate aggregate);

        public string AggregateId { get; set; }
        public TProperty OldValue { get; set; }
        public TProperty NewValue { get; set; }
    }
}