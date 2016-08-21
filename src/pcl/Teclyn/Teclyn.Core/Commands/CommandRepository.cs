using System.Collections.Generic;

namespace Teclyn.Core.Commands
{
    public class CommandRepository
    {
        private readonly IDictionary<string, CommandInfo> commands = new Dictionary<string, CommandInfo>();

        public IEnumerable<CommandInfo> Commands => this.commands.Values;

        public void RegisterCommand(CommandInfo commandInfo)
        {
            this.commands.Add(commandInfo.Id, commandInfo);
        }

        public CommandInfo Get(string id)
        {
            return this.commands[id];
        }
    }
}