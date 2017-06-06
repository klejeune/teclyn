using System.Linq;

namespace Teclyn.Core.Queries
{
    public class UserFriendlyQueryResult<TResult> : IUserFriendlyQueryResult
    {
        public bool Success { get; }

        public QueryResultError[] Errors { get; }

        public TResult Result { get; }

        public UserFriendlyQueryResult(QueryExecutionResult<TResult> result)
        {
            this.Success = result.Success;
            this.Errors = result.Errors.ToArray();
            this.Result = result.Result;
        }
    }
}