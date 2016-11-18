using System.Collections.Generic;
using Teclyn.Core.Commands;

namespace Teclyn.Core.Metadata
{
    public class MetadataRepository
    {
        private readonly IDictionary<string, CommandInfo> commands = new Dictionary<string, CommandInfo>();
        private readonly IDictionary<string, EventInfo> events = new Dictionary<string, EventInfo>();

        public IEnumerable<CommandInfo> Commands => this.commands.Values;
        public IEnumerable<EventInfo> Events => this.events.Values;

        public void RegisterCommand(CommandInfo commandInfo)
        {
            this.commands.Add(commandInfo.Id, commandInfo);
        }

        public void RegisterEvent(EventInfo eventInfo)
        {
            this.events.Add(eventInfo.Id, eventInfo);
        }

        public CommandInfo GetCommand(string id)
        {
            return this.commands[id];
        }

        public EventInfo GetEvent(string id)
        {
            return this.events[id];
        }
    }
}