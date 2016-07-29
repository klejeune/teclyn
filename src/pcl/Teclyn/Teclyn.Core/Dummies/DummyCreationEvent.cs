using Teclyn.Core.Events;

namespace Teclyn.Core.Dummies
{
    public class DummyCreationEvent : ICreationEvent<IDummyAggregate>
    {
        public IDummyAggregate Apply(IEventInformation information)
        {
            return new DummyAggregate(this);
        }

        public string AggregateId { get; set; }
    }
}