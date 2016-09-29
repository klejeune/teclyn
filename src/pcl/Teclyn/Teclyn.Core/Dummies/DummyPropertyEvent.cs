using Teclyn.Core.Events;
using Teclyn.Core.Events.Properties;

namespace Teclyn.Core.Dummies
{
    public class DummyPropertyEvent : IPropertyEvent<IDummyAggregate, string>
    {
        public void Apply(IDummyAggregate aggregate, IEventInformation information)
        {
            aggregate.Update(information.Type(this));
        }

        public string AggregateId { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }
}