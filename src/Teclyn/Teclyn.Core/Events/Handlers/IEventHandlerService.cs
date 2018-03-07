using System;
using System.Collections.Generic;
using Teclyn.Core.Domains;
using Teclyn.Core.Events.Metadata;

namespace Teclyn.Core.Events.Handlers
{
    [Service]
    public interface IEventHandlerService
    {
        IReadOnlyDictionary<Type, IEnumerable<EventHandlerMetadata>> GetEventHandlers();
        IEnumerable<EventHandlerMetadata> GetEventHandlers(Type eventType);
        void RegisterEventHandler(Type eventHandlerType);
    }
}