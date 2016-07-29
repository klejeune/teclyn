namespace Teclyn.Core.Dummies
{
    public class DummyAggregate : IDummyAggregate
    {
        public DummyAggregate()
        {
            
        }

        public DummyAggregate(DummyCreationEvent @event)
        {
            this.Id = @event.AggregateId;
        }

        public string Id { get; set; }
    }
}