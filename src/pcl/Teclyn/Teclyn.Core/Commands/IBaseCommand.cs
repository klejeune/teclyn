using Teclyn.Core.Security.Context;

namespace Teclyn.Core.Commands
{
    public interface IBaseCommand
    {
        bool CheckParameters(IParameterChecker _);
        bool CheckContext(ITeclynContext context, ICommandContextChecker _);

        void Execute(ICommandExecutionContext context);
    }

    public interface ICommand : IBaseCommand
    {
        
    }

    public interface ICommand<out TResult> : IBaseCommand
    {
        TResult Result { get; }
    }
}