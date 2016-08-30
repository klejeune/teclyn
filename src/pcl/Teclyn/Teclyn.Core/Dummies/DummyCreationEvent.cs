using Teclyn.Core.Events;

namespace Teclyn.Core.Dummies
{
    public class DummyCreationEvent : IEvent<IDummyAggregate>
    {
        public void Apply(IDummyAggregate aggregate, IEventInformation information)
        {
            aggregate.Create(information.Type(this));
        }

        public string AggregateId { get; set; }
    }
}