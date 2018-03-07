using Teclyn.Core.Events;
using Teclyn.Core.Events.Properties;

namespace Teclyn.Core.Dummies
{
    public class DummyPropertyEvent : IPropertyEvent<IDummyAggregate, string>
    {
        public void Apply(IDummyAggregate aggregate)
        {
            aggregate.Update(this);
        }

        public string AggregateId { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }
}