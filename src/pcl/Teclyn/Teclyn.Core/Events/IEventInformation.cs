using System;
using Teclyn.Core.Security.Context;

namespace Teclyn.Core.Events
{
    public interface IEventInformation
    {
        DateTime Date { get; }
        ITeclynUser User { get; }
        IEventInformation<TEvent> Type<TEvent>(TEvent @event) where TEvent : ITeclynEvent;
    }

    public interface IEventInformation<out TEvent> : IEventInformation where TEvent : ITeclynEvent
    {
        TEvent Event { get; }
    }
}