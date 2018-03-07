using Teclyn.Core.Api;
using Teclyn.Core.Ioc;

namespace Teclyn.Core.Queries
{
    public interface IQueryExecutionContext<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        IDependencyResolver DependencyResolver { get; }
        QueryResultMetadata<TQuery, TResult> Metadata { get; }
    }
}