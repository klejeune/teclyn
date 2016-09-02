using System;
using Teclyn.Core.Domains;
using Teclyn.Core.Events;
using Teclyn.Core.Events.Properties;
using Teclyn.Core.Ioc;
using Teclyn.Core.Security.Context;
using Teclyn.Core.Storage;

namespace Teclyn.Core.Commands.Properties
{
    public abstract class AbstractPropertyCommand<TAggregate, TProperty> : IPropertyCommand<TAggregate, TProperty>
    where TAggregate : class, IAggregate
    {
        public abstract bool CheckParameters(IParameterChecker _);

        public abstract bool CheckContext(ITeclynContext context, ICommandContextChecker _);

        public abstract void Execute(ICommandExecutionContext context);

        public TProperty NewValue { get; set; }
        public string AggregateId { get; set; }

        public abstract Func<TAggregate, TProperty> PropertyAccessor { get; }
    }

    public abstract class PropertyCommand<TAggregate, TEvent, TProperty> : AbstractPropertyCommand<TAggregate, TProperty>
        where TAggregate : class, IAggregate
        where TEvent : IPropertyEvent<TAggregate, TProperty>
    {
        public override void Execute(ICommandExecutionContext context)
        {
            var repository = context.Teclyn.Get<IRepository<TAggregate>>();
            var aggregate = repository.GetById(this.AggregateId);
            var oldValue = this.PropertyAccessor(aggregate);

            if (oldValue == null && this.NewValue != null || oldValue != null && !oldValue.Equals(NewValue))
            {
                var @event = Activator.CreateInstance<TEvent>();
                @event.OldValue = oldValue;
                @event.NewValue = this.NewValue;
                @event.AggregateId = this.AggregateId;

                context.GetEventService().Raise(@event);
            }
        }
    }
}