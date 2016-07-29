using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        private Action<IAggregate, IEventInformation> action;
        public EventHandlerMetadata(Type eventHandlerType, Type eventType, Action<IAggregate, IEventInformation> action)
        {
            this.EventHandlerType = eventHandlerType;
            this.EventType = eventType;
            this.Name = eventHandlerType.Name;
            this.action = action;
        }

        public Action GetHandleAction(IAggregate aggregate, ITeclynEvent @event, IEventInformation eventInformation)
        {
            return () => this.action(aggregate, eventInformation);
        }
    }
}