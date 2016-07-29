namespace Teclyn.Core.Commands
{
    public class CommandExecutionContext : ICommandExecutionContext
    {
        public CommandExecutionContext(TeclynApi teclyn)
        {
            this.Teclyn = teclyn;
        }

        public TeclynApi Teclyn { get; }
    }
}