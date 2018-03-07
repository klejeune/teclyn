using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Teclyn.Core.Api;
using Teclyn.Core.Commands;
using Teclyn.Core.Ioc;
using Teclyn.Core.Security.Context;
using Teclyn.Core.Tools;

namespace Teclyn.Core.Queries
{
    public class QueryService
    {
        private readonly ITeclynContext _context;
        private readonly IDependencyResolver _dependencyResolver;

        public QueryService(ITeclynContext context, IDependencyResolver dependencyResolver)
        {
            this._context = context;
            this._dependencyResolver = dependencyResolver;
        }

        public async Task<IQueryResult<IQuery<TResult>, TResult>> Execute<TResult>(IQuery<TResult> query)
        {
            var method = ReflectionTools.Instance<QueryService>
                .Method(service => service.Execute<Dummies.DummyQuery, Dummies.DummyQueryResult>(null))
                .GetGenericMethodDefinition()
                .MakeGenericMethod(query.GetType(), typeof(TResult));

            return await (Task<IQueryResult<IQuery<TResult>, TResult>>) method.Invoke(this, new object[] {query});
        }

        public async Task<IQueryResult<TQuery, TResult>> Execute<TQuery, TResult>(TQuery query) where TQuery: IQuery<TResult>
        {
            var queryHandler = this._dependencyResolver.Get<IQueryHandler<TQuery, TResult>>();
            var result = new QueryExecutionResult<TQuery, TResult>(this._dependencyResolver);

            var success = await queryHandler.CheckContext(query, this._context, result)
                && await queryHandler.CheckParameters(query, result)
                && await this.ExecuteQueryHandler(query, queryHandler, result);

            return result;
        }

        private async Task<bool> ExecuteQueryHandler<TQuery, TResult>(TQuery command, IQueryHandler<TQuery, TResult> queryHandler, QueryExecutionResult<TQuery, TResult> result) where TQuery : IQuery<TResult>
        {
            try
            {
                var handlerResult = await queryHandler.Execute(command, result);

                result.SetSuccess();
                result.Result = handlerResult;

                return true;
            }
            catch (Exception exception)
            {
                result.SetFailure(exception.ToString());

                return false;
            }
        }

        public IDictionary<string, object> Serialize(ICommand command)
        {
            var properties = command.GetType().GetRuntimeProperties().Where(p => p.CanRead && p.CanWrite);

            return properties.ToDictionary(p => p.Name, p => p.GetValue(command));
        }
    }
}
