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

        public static ICommandResult<TResult> Execute<TCommand, TResult>(this TCommand command, CommandService commandService)
            where TCommand : ICommand<TResult>
        {
            return commandService.Execute(command);
        }

        public static ICommandResult Execute<TCommand>(this TCommand command, CommandService commandService)
        where TCommand : ICommand
        {
            return commandService.Execute(command);
        }
    }
}