using Teclyn.Core.Events;

namespace Teclyn.Core.Dummies
{
    public class DummyCreationEvent : IEvent<IDummyAggregate>
    {
        public void Apply(IDummyAggregate aggregate)
        {
            aggregate.Create(this);
        }

        public string AggregateId { get; set; }
    }
}