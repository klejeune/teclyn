using Teclyn.Core.Commands;
using Teclyn.Core.Domains;
using Teclyn.Core.Errors.Commands;
using Teclyn.Core.Ioc;

namespace Teclyn.Core.Errors
{
    [Service]
    public class ErrorManager
    {
        [Inject]
        public CommandService CommandService { get; set; }

        public void LogError(string message, string description)
        {
            var command = this.CommandService.Create<LogErrorCommand>();
            command.Message = message;
            command.Description = description;
            command.Execute(this.CommandService);
        }
    }
}