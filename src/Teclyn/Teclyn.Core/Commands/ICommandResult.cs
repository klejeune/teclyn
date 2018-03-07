using System.Collections.Generic;

namespace Teclyn.Core.Commands
{
    public interface ICommandResult
    {
        bool Success { get; }
        IEnumerable<CommandResultError> Errors { get; }

        bool ContextIsValid { get; }
        bool ParametersAreValid { get; }
    }
}