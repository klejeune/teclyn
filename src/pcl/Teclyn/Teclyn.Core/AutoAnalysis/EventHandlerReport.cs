using System.Collections.Generic;
using System.Reflection;

namespace Teclyn.Core.AutoAnalysis
{
    public class EventHandlerReport
    {
        public string Type { get; }

        public EventHandlerReport(TypeInfo type)
        {
            this.Type = type.Name;
        }
    }
}