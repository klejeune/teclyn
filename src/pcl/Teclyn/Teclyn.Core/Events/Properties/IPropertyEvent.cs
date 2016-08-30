using System.Diagnostics.Contracts;
using Teclyn.Core.Domains;

namespace Teclyn.Core.Events.Properties
{
    public interface IPropertyEvent
    {
        
    }

    public interface IPropertyEvent<TAggregate, TProperty> : IPropertyEvent, IEvent<TAggregate> where TAggregate : IAggregate
    {
        TProperty OldValue { get; set; }
        TProperty NewValue { get; set; }
    }
}