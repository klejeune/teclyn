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

        public static ICommandResult<TResult> Execute<TCommand, TResult>(this TCommand command, TeclynApi teclynApi)
            where TCommand : ICommand<TResult>
        {
            return teclynApi.Get<CommandService>().Execute<TCommand, TResult>(command);
        }

        public static ICommandResult Execute<TCommand>(this TCommand command, TeclynApi teclynApi)
        where TCommand : ICommand
        {
            return teclynApi.Get<CommandService>().Execute<TCommand>(command);
        }
    }
}