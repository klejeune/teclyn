using System;
using System.Collections.Generic;

namespace Teclyn.Core.Events.Metadata
{
    public class EventMetadata
    {
        public string Name { get; }
        public Type Type { get; }
        public IEnumerable<EventHandlerMetadata> EventHandlers { get; }
    }
}