using System.Linq;

namespace Teclyn.Core.Commands
{
    public class UserFriendlyCommandResult<TResult> : IUserFriendlyCommandResult
    {
        public bool Success { get; }

        public CommandResultError[] Errors { get; }

        public TResult Result { get; }

        public UserFriendlyCommandResult(CommandExecutionResult<TResult> result)
        {
            this.Success = result.Success;
            this.Errors = result.Errors.ToArray();
            this.Result = result.Result;
        }
    }
}