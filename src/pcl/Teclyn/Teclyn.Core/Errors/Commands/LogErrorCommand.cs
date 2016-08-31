using Teclyn.Core.Commands;
using Teclyn.Core.Errors.Events;
using Teclyn.Core.Security.Context;

namespace Teclyn.Core.Errors.Commands
{
    public class LogErrorCommand : ICommand
    {
        public string Message { get; set; }
        public string Description { get; set; }

        public bool CheckParameters(IParameterChecker _)
        {
            return true;
        }

        public bool CheckContext(ITeclynContext context, ICommandContextChecker _)
        {
            return true;
        }

        public void Execute(ICommandExecutionContext context)
        {
            context.GetEventService().Raise(new ErrorLoggedEvent
            {
                Message = this.Message,
                AggregateId = context.GetIdGenerator().GenerateId(),
                Description = this.Description,
            });
        }
    }
}