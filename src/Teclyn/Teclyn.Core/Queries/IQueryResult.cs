using System.Collections.Generic;

namespace Teclyn.Core.Queries
{
    public interface IQueryResult
    {
        bool Success { get; }
        IEnumerable<QueryResultError> Errors { get; }

        bool ContextIsValid { get; }
        bool ParametersAreValid { get; }
        object GetResult();
    }

    public interface IQueryResult<TQuery, TResult> : IQueryResult where TQuery : IQuery<TResult>
    {
        TResult Result { get; }
        QueryResultMetadata<TQuery, TResult> Metadata { get; }
    }
}