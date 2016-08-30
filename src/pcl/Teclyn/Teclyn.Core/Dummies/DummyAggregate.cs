using Teclyn.Core.Events;

namespace Teclyn.Core.Dummies
{
    public class DummyAggregate : IDummyAggregate
    {
        public string Id { get; set; }

        public void Create(IEventInformation<DummyCreationEvent> eventInformation)
        {
            this.Id = eventInformation.Event.AggregateId;
        }
    }
}