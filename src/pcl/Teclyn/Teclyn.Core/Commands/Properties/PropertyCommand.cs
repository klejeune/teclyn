using System;
using Teclyn.Core.Domains;
using Teclyn.Core.Events;
using Teclyn.Core.Events.Properties;
using Teclyn.Core.Ioc;
using Teclyn.Core.Security.Context;
using Teclyn.Core.Storage;

namespace Teclyn.Core.Commands.Properties
{
    public abstract class PropertyCommand<TAggregate, TEvent, TProperty> : IPropertyCommand<TAggregate, TProperty>
        where TAggregate : class, IAggregate
        where TEvent : IPropertyEvent<TAggregate, TProperty>
    {
        public abstract bool CheckParameters(IParameterChecker _);

        public abstract bool CheckContext(ITeclynContext context, ICommandContextChecker _);

        public void Execute(ICommandExecutionContext context)
        {
            var repository = context.Teclyn.Get<IRepository<TAggregate>>();
            var @event = Activator.CreateInstance<TEvent>();
            var aggregate = repository.GetById(this.AggregateId);
            @event.OldValue = this.PropertyAccessor(aggregate);
            @event.NewValue = this.NewValue;
            @event.AggregateId = this.AggregateId;

            context.GetEventService().Raise(@event);
        }

        public TProperty NewValue { get; set; }
        public string AggregateId { get; set; }

        protected abstract Func<TAggregate, TProperty> PropertyAccessor { get; }
    }
}