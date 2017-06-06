using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teclyn.Core.Security.Context;

namespace Teclyn.Core.Queries
{
    public interface IQuery
    {
        bool CheckContext(ITeclynContext context, QueryExecutionResult result);
        bool CheckParameters(QueryExecutionResult result);
        Task Execute(IQueryExecutionContext context);
    }

    public interface IQuery<out TResult> : IQuery
    {
        TResult Result { get; }
    }
}
