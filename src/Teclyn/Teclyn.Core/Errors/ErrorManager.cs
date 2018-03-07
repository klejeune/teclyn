using System.Threading.Tasks;
using Teclyn.Core.Commands;
using Teclyn.Core.Domains;
using Teclyn.Core.Errors.Commands;
using Teclyn.Core.Ioc;

namespace Teclyn.Core.Errors
{
    public class ErrorManager
    {
        private readonly CommandService _commandService;

        public ErrorManager(CommandService commandService)
        {
            this._commandService = commandService;
        }

        public async Task LogError(string message, string description)
        {
            await this._commandService.Execute(new LogErrorCommand
            {
                Message = message,
                Description = description
            });
        }
    }
}