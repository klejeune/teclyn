using System.Collections.Generic;
using System.Reflection;

namespace Teclyn.Core.AutoAnalysis
{
    public class AutoAnalysisReport
    {
        public IEnumerable<CommandReport> Commands { get; }
        public IEnumerable<EventReport> Events { get; }
        public IEnumerable<EventHandlerReport> EventHandlers { get; }

        public AutoAnalysisReport(IEnumerable<CommandReport> commands, IEnumerable<EventReport> events, IEnumerable<EventHandlerReport> eventHandlers)
        {
            this.Commands = commands;
            this.Events = events;
            this.EventHandlers = eventHandlers;
        }
    }
}