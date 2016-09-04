using System;
using System.Linq;
using Teclyn.Core.Domains;
using Teclyn.Core.Events;
using Teclyn.Core.Storage;
using Xunit;

namespace Teclyn.Core.Tests.Events
{
    public class EventServiceTests
    {
        [Aggregate]
        private class DummyAggregate : IAggregate
        {
            public string Id { get; set; }
            public string Value { get; set; }

            public void Modify(DummyModificationEvent @event)
            {
                this.Value = @event.Value;
            }

            public void Create(IEventInformation<DummyCreationEvent> eventInformation)
            {
                this.Id = eventInformation.Event.AggregateId;
            }
        }

        private class DummyCreationEvent : IEvent<DummyAggregate>
        {
            public void Apply(DummyAggregate aggregate, IEventInformation information)
            {
                aggregate.Create(information.Type(this));
            }

            public string AggregateId { get; set; }
        }

        private class DummyModificationEvent : IEvent<DummyAggregate>
        {
            public string Value { get; set; }

            public void Apply(DummyAggregate aggregate, IEventInformation information)
            {
                aggregate.Modify(this);
            }

            public string AggregateId { get; set; }
        }

        private class DummySuppressionEvent : ISuppressionEvent<DummyAggregate>
        {
            public string AggregateId { get; set; }
        }

        private readonly EventService eventService;
        private readonly IRepository<DummyAggregate> repository;

        public EventServiceTests()
        {
            var teclyn = TeclynApi.Initialize(new TeclynTestConfiguration());
            this.eventService = teclyn.Get<EventService>();
            this.repository = teclyn.Get<IRepository<DummyAggregate>>();
        }

        [Fact]
        public void ObjectIsCreated()
        {
            var aggregateId = "myAggregateId";

            var createdAggregate = this.eventService.Raise(new DummyCreationEvent
            {
                AggregateId = aggregateId
            });

            Assert.Equal(aggregateId, createdAggregate.Id);
            Assert.NotNull(this.repository.GetByIdOrNull(aggregateId));
        }

        [Fact]
        public void ObjectIsModified()
        {
            var aggregateId = "myAggregateId";
            var value = "my-value";

            var createdAggregate = this.eventService.Raise(new DummyCreationEvent
            {
                AggregateId = aggregateId
            });
            this.eventService.Raise(new DummyModificationEvent
            {
                AggregateId = aggregateId,
                Value = value,
            });

            Assert.Equal(aggregateId, createdAggregate.Id);
            Assert.Equal(value, createdAggregate.Value);
            Assert.NotNull(this.repository.GetByIdOrNull(aggregateId));
        }

        [Fact]
        public void ObjectIsDeleted()
        {
            var aggregateId = "myAggregateId";

            this.eventService.Raise(new DummyCreationEvent
            {
                AggregateId = aggregateId
            });
            this.eventService.Raise(new DummySuppressionEvent
            {
                AggregateId = aggregateId,
            });


            Assert.Null(this.repository.GetByIdOrNull(aggregateId));
        }
    }
}