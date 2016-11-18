using System;
using Teclyn.Core.Domains;

namespace Teclyn.Core.Metadata
{
    public class EventInfo : IDisplayable
    {
        public Type EventType { get; }
        public string Name { get; }
        public string Id { get; }

        public EventInfo(string id, string name, Type eventType)
        {
            this.Id = id;
            this.Name = name;
            this.EventType = eventType;
        }
    }
}