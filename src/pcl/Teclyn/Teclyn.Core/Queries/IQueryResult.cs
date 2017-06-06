using System.Collections.Generic;

namespace Teclyn.Core.Queries
{
    public interface IQueryResult
    {
        bool Success { get; }
        IEnumerable<QueryResultError> Errors { get; }

        bool ContextIsValid { get; }
        bool ParametersAreValid { get; }
        IUserFriendlyQueryResult ToUserFriendly();
    }

    public interface IQueryResult<TResult> : IQueryResult
    {
        TResult Result { get; }
    }
}