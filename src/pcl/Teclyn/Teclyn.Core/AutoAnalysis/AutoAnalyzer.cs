using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Teclyn.Core.Commands;
using Teclyn.Core.Domains;
using Teclyn.Core.Events;
using Teclyn.Core.Events.Handlers;
using Teclyn.Core.Ioc;
using Teclyn.Core.Storage;

namespace Teclyn.Core.AutoAnalysis
{
    [Service]
    public class AutoAnalyzer
    {
        [Inject]
        public RepositoryService RepositoryService { get; set; }

        [Inject]
        public TeclynApi Teclyn { get; set; }

        [Inject]
        public EventHandlerService EventHandlerService { get; set; }

        private TypeInfo commandType = typeof(ICommand).GetTypeInfo();
        private TypeInfo eventType = typeof(ITeclynEvent).GetTypeInfo();
        private TypeInfo handlerType = typeof(IEventHandler).GetTypeInfo();

        public AutoAnalysisReport Analyze()
        {
            var types = this.Teclyn.Plugins
                .Select(plugin => plugin.GetType().GetTypeInfo().Assembly)
                .Distinct()
                .SelectMany(assembly => assembly.DefinedTypes)
                .Where(type => type.Namespace == null || !type.Namespace.StartsWith("Teclyn.Core."))
                .ToList();

            var commands = new Dictionary<TypeInfo, CommandReport>();
            var events = new Dictionary<TypeInfo, EventReport>();
            var handlers = this.EventHandlerService.GetEventHandlers().Select(pair => new EventHandlerReport(pair.Key.GetTypeInfo())).ToList();

            foreach (var type in types)
            {
                if (commandType.IsAssignableFrom(type))
                {
                    commands.Add(type, new CommandReport(type));
                }

                if (eventType.IsAssignableFrom(type))
                {
                    events.Add(type, new EventReport(type));
                }
            }

            return new AutoAnalysisReport(commands.Values, events.Values, handlers);
        }
    }
}