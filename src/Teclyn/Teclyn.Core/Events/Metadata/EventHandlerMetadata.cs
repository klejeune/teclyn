using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Teclyn.Core.Domains;
using Teclyn.Core.Dummies;
using Teclyn.Core.Events.Handlers;
using Teclyn.Core.Ioc;
using Teclyn.Core.Tools;

namespace Teclyn.Core.Events.Metadata
{
    public class EventHandlerMetadata
    {
        public string Name { get; }
        public Type EventHandlerType { get; }
        public Type EventType { get; }

        private Func<IAggregate, ITeclynEvent, Task> action;
        public EventHandlerMetadata(Type eventHandlerType, Type eventType, Func<IAggregate, ITeclynEvent, Task> action)
        {
            this.EventHandlerType = eventHandlerType;
            this.EventType = eventType;
            this.Name = eventHandlerType.Name;
            this.action = action;
        }

        public Func<Task> GetHandleAction(IAggregate aggregate, ITeclynEvent @event)
        {
            return () => this.action(aggregate, @event);
        }
    }
}