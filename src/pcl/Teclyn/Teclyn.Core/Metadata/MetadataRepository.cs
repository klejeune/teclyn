using System;
using System.Collections.Generic;
using System.Reflection;
using Teclyn.Core.Commands;

namespace Teclyn.Core.Metadata
{
    public class MetadataRepository
    {
        private readonly IDictionary<string, CommandInfo> commands = new Dictionary<string, CommandInfo>();
        private readonly IDictionary<string, EventInfo> events = new Dictionary<string, EventInfo>();

        public IEnumerable<CommandInfo> Commands => this.commands.Values;
        public IEnumerable<EventInfo> Events => this.events.Values;

        public void RegisterCommand(Type commandType)
        {
            if (!typeof(IBaseCommand).GetTypeInfo().IsAssignableFrom(commandType.GetTypeInfo()))
            {
                throw new TeclynException($"Unable to register type {commandType.Name}: it is not a command type. (It doesn't implement ICommand.)");
            }

            this.RegisterCommand(new CommandInfo(commandType.Name.ToLowerInvariant(), commandType.Name, commandType));
        }

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