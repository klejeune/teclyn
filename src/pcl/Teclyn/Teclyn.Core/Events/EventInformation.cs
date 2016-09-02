using System;
using Teclyn.Core.Security.Context;

namespace Teclyn.Core.Events
{
    public class EventInformation<TEvent> : IEventInformation<TEvent> where TEvent : ITeclynEvent
    {
        public TEvent Event { get; set; }
        public DateTime Date { get; set; }
        public ITeclynUser User { get; set; }
        public string EventType { get; set; }

        public IEventInformation<TEvent1> Type<TEvent1>(TEvent1 @event) where TEvent1 : ITeclynEvent
        {
            return (IEventInformation<TEvent1>) this;
        }

        public string Id { get; set; }
    }
}