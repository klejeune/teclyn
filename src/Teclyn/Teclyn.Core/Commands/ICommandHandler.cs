using System.Threading.Tasks;
using Teclyn.Core.Security.Context;

namespace Teclyn.Core.Commands
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        Task<bool> CheckParameters(TCommand command, IParameterChecker _);
        Task<bool> CheckContext(TCommand command, ITeclynContext context, ICommandContextChecker _);

        Task Execute(TCommand command, ICommandExecutionContext context);
    }
}