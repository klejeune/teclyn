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

        public static ICommandResult<TResult> Execute<TResult>(this ICommand<TResult> command, CommandService commandService)
        {
            return commandService.Execute(command);
        }

        public static ICommandResult Execute(this ICommand command, CommandService commandService)
        {
            return commandService.Execute(command);
        }
    }
}