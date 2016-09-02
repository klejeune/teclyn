using System;
using Teclyn.Core.Domains;
using Teclyn.Core.Security.Context;

namespace Teclyn.Core.Events
{
    public interface IEventInformation : IAggregate
    {
        DateTime Date { get; }
        ITeclynUser User { get; }
        string EventType { get; }
        IEventInformation<TEvent> Type<TEvent>(TEvent @event) where TEvent : ITeclynEvent;
    }

    public interface IEventInformation<out TEvent> : IEventInformation where TEvent : ITeclynEvent
    {
        TEvent Event { get; }
    }
}