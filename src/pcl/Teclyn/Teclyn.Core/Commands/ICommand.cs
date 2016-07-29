using Teclyn.Core.Security.Context;

namespace Teclyn.Core.Commands
{
    public interface ICommand
    {
        bool CheckParameters(IParameterChecker _);
        bool CheckContext(ITeclynContext context, ICommandContextChecker _);

        void Execute(ICommandExecutionContext context);
    }

    public interface ICommand<out TResult> : ICommand
    {
        TResult Result { get; }
    }
}