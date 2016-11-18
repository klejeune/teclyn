using System.Collections.Generic;
using System.Reflection;

namespace Teclyn.Core.AutoAnalysis
{
    public class EventReport
    {
        public EventReport(TypeInfo type)
        {
            this.Type = type.Name;
        }

        public string Type { get; }
        public IEnumerable<EventHandlerReport> Handlers => this.handlers;
        private readonly List<EventHandlerReport> handlers = new List<EventHandlerReport>();

        public void RegisterHandler(EventHandlerReport report)
        {
            this.handlers.Add(report);
        }
    }
}