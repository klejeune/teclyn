using System;
using Teclyn.Core.Domains;

namespace Teclyn.Core.Commands.Properties
{
    public interface IPropertyCommand : ICommand
    {
        
    }

    public interface IPropertyCommand<in TAggregate, TProperty> : IPropertyCommand where TAggregate : IAggregate
    {
        string AggregateId { get; set; }
        TProperty NewValue { get; set; }
        Func<TAggregate, TProperty> PropertyAccessor { get; }
    }
}