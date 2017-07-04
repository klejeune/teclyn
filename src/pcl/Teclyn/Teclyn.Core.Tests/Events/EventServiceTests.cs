using System;
using System.Linq;
using System.Threading.Tasks;
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

            public void Create(DummyCreationEvent eventInformation)
            {
                this.Id = eventInformation.AggregateId;
            }
        }

        private class DummyCreationEvent : IEvent<DummyAggregate>
        {
            public void Apply(DummyAggregate aggregate)
            {
                aggregate.Create(this);
            }

            public string AggregateId { get; set; }
        }

        private class DummyModificationEvent : IEvent<DummyAggregate>
        {
            public string Value { get; set; }

            public void Apply(DummyAggregate aggregate)
            {
                aggregate.Modify(this);
            }

            public string AggregateId { get; set; }
        }

        private class DummySuppressionEvent : ISuppressionEvent<DummyAggregate>
        {
            public string AggregateId { get; set; }
        }

        private readonly IEventService eventService;
        private readonly IRepository<DummyAggregate> repository;

        public EventServiceTests()
        {
            var teclyn = new TeclynApi(new TeclynTestConfiguration());
            this.eventService = teclyn.Get<IEventService>();
            this.repository = teclyn.Get<IRepository<DummyAggregate>>();
        }

        [Fact]
        public async void ObjectIsCreated()
        {
            var aggregateId = "myAggregateId";

            var createdAggregate = await this.eventService.Raise(new DummyCreationEvent
            {
                AggregateId = aggregateId
            });

            Assert.Equal(aggregateId, createdAggregate.Id);
            Assert.NotNull(this.repository.GetByIdOrNull(aggregateId));
        }

        [Fact]
        public async void ObjectIsModified()
        {
            var aggregateId = "myAggregateId";
            var value = "my-value";

            var createdAggregate = await this.eventService.Raise(new DummyCreationEvent
            {
                AggregateId = aggregateId
            });
            await this.eventService.Raise(new DummyModificationEvent
            {
                AggregateId = aggregateId,
                Value = value,
            });

            Assert.Equal(aggregateId, createdAggregate.Id);
            Assert.Equal(value, createdAggregate.Value);
            Assert.NotNull(this.repository.GetByIdOrNull(aggregateId));
        }

        [Fact]
        public async void ObjectIsDeleted()
        {
            var aggregateId = "myAggregateId";

            await this.eventService.Raise(new DummyCreationEvent
            {
                AggregateId = aggregateId
            });
            await this.eventService.Raise(new DummySuppressionEvent
            {
                AggregateId = aggregateId,
            });

            Assert.Null(await this.repository.GetByIdOrNull(aggregateId));
        }
    }
}