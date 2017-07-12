using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Teclyn.Core.Commands;
using Teclyn.Core.Commands.Properties;
using Teclyn.Core.Domains;
using Teclyn.Core.Events;
using Teclyn.Core.Events.Properties;
using Teclyn.Core.Security.Context;
using Teclyn.Core.Storage;
using Xunit;

namespace Teclyn.Core.Tests.Commands
{
    public class PropertyCommandTests
    {
        public class DummyAggregate : IAggregate
        {
            public string Id { get; set; }
            public string Value { get; set; }
            
            public void UpdateValue(DummyValueUpdatedEvent dummyValueUpdatedEvent)
            {
                this.Value = dummyValueUpdatedEvent.NewValue;
            }
        }

        public class DummyValueUpdatedEvent : IPropertyEvent<DummyAggregate, string>
        {
            public void Apply(DummyAggregate aggregate)
            {
                aggregate.UpdateValue(this);
            }

            public string AggregateId { get; set; }
            public string OldValue { get; set; }
            public string NewValue { get; set; }
        }

        public class UpdateDummyValueCommand : PropertyCommand<DummyAggregate, DummyValueUpdatedEvent, string>
        {
            private bool contextOk = true;

            public void InvalidateContext()
            {
                this.contextOk = false;
            }

            public override bool CheckParameters(IParameterChecker _)
            {
                return _.Check(!string.IsNullOrWhiteSpace(this.NewValue), "The new value cannot be empty.");
            }

            public override bool CheckContext(ITeclynContext context, ICommandContextChecker _)
            {
                return _.Check(contextOk, "You are not allowed to execute this command.");
            }

            public override Func<DummyAggregate, string> PropertyAccessor => aggregate => aggregate.Value;
        }

        private TeclynApi teclyn;
        private CommandService commandService;
        private string aggregateId = "id";
        private DummyAggregate aggregate;
        private RepositoryService repositoryService;

        public PropertyCommandTests()
        {
            this.teclyn = new TeclynApi(new TeclynTestConfiguration());
            this.repositoryService = teclyn.Get<RepositoryService>();
            this.commandService = teclyn.Get<CommandService>();

            this.repositoryService.Register(typeof(DummyAggregate), typeof(DummyAggregate), typeof(DummyAggregate).Name, null, null);

            this.aggregate = new DummyAggregate
            {
                Value = "value",
                Id = aggregateId,
            };

            var repository = this.teclyn.Get<IRepository<DummyAggregate>>();
            repository.Create(aggregate);
        }

        //[Fact]
        public async Task PropertyCommandIsExecuted()
        {
            var newValue = "newValue";
            
            var command = new UpdateDummyValueCommand
            {
                AggregateId = aggregateId,
                NewValue = newValue
            };

            var result = await command.Execute(this.commandService);

            Assert.True(result.Success);
            Assert.True(result.ContextIsValid);
            Assert.True(result.ParametersAreValid);
            Assert.False(result.Errors.Any());
            Assert.Equal(newValue, this.aggregate.Value);
        }

        [Fact]
        public async Task ResultWithBadContextFails()
        {
            var oldValue = this.aggregate.Value;
            var newValue = oldValue + "NEVERAFFECTED";

            var command = new UpdateDummyValueCommand
            {
                AggregateId = aggregateId,
                NewValue = newValue
            };

            command.InvalidateContext();

            var result = await command.Execute(this.commandService);

            Assert.False(result.Success);
            Assert.False(result.ContextIsValid);
            Assert.True(result.ParametersAreValid);
            Assert.True(result.Errors.Any());
            Assert.Equal(oldValue, this.aggregate.Value);
            Assert.NotEqual(newValue, this.aggregate.Value);
        }

        [Fact]
        public async Task ResultWithBadParameterFails()
        {
            var oldValue = this.aggregate.Value;
            var newValue = string.Empty;

            var command = new UpdateDummyValueCommand
            {
                AggregateId = aggregateId,
                NewValue = newValue
            };
            
            var result = await command.Execute(this.commandService);

            Assert.False(result.Success);
            Assert.False(result.ParametersAreValid);
            Assert.True(result.ContextIsValid);
            Assert.True(result.Errors.Any());
            Assert.Equal(oldValue, this.aggregate.Value);
            Assert.NotEqual(newValue, this.aggregate.Value);
        }
    }
}