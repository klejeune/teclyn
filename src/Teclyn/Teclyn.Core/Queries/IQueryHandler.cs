using System.Threading.Tasks;
using Teclyn.Core.Commands;
using Teclyn.Core.Security.Context;

namespace Teclyn.Core.Queries
{
    public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        Task<bool> CheckContext(TQuery query, ITeclynContext context, IQueryContextChecker result);
        Task<bool> CheckParameters(TQuery query, IParameterChecker result);
        Task<TResult> Execute(TQuery query, IQueryExecutionContext<TQuery, TResult> context);
    }
}