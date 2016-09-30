using System.Threading.Tasks;
using Teclyn.Core.Events;
using Teclyn.Core.Tools;

namespace Teclyn.Core.Commands
{
    public static class CommandExtensions
    {
        public static EventService GetEventService(this ICommandExecutionContext context)
        {
            return context.Teclyn.Get<EventService>();
        }

        public static IdGenerator GetIdGenerator(this ICommandExecutionContext context)
        {
            return context.Teclyn.Get<IdGenerator>();
        }

        public static async Task<ICommandResult<TResult>> Execute<TResult>(this ICommand<TResult> command, CommandService commandService)
        {
            return await commandService.Execute(command);
        }

        public static async Task<ICommandResult> Execute(this ICommand command, CommandService commandService)
        {
            return await commandService.Execute(command);
        }
    }
}