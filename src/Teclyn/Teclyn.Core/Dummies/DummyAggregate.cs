using Teclyn.Core.Events;

namespace Teclyn.Core.Dummies
{
    public class DummyAggregate : IDummyAggregate
    {
        public string Id { get; set; }
        public string Property { get; set; }

        public void Create(DummyCreationEvent eventInformation)
        {
            this.Id = eventInformation.AggregateId;
        }

        public void Update(DummyPropertyEvent eventInformation)
        {
            this.Property = eventInformation.NewValue;
        }
    }
}