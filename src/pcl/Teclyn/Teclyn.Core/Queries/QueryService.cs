using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Teclyn.Core.Commands;
using Teclyn.Core.Ioc;
using Teclyn.Core.Metadata;
using Teclyn.Core.Security.Context;

namespace Teclyn.Core.Queries
{
    public class QueryService
    {
        private readonly ITeclynContext context;
        private readonly TeclynApi teclyn;
        private readonly IIocContainer iocContainer;
        private readonly MetadataRepository metadataRepository;

        public QueryService(ITeclynContext context, TeclynApi teclyn, IIocContainer iocContainer, MetadataRepository metadataRepository)
        {
            this.context = context;
            this.teclyn = teclyn;
            this.iocContainer = iocContainer;
            this.metadataRepository = metadataRepository;
        }
        
        public async Task<IQueryResult<TResult>> Execute<TResult>(IQuery<TResult> query)
        {
            return await this.ExecuteInternal(query, command1 => command1.Result);
        }

        public async Task<IQueryResult<TResult>> Execute<TQuery, TResult>(Action<TQuery> builder) where TQuery : IQuery<TResult>
        {
            var command = this.Create(builder);
            return await this.Execute(command);
        }
        
        public async Task<IUserFriendlyQueryResult> ExecuteGeneric(IQuery query)
        {
            var result = await this.ExecuteInternal(query, command1 => string.Empty);
            return result.ToUserFriendly();
        }

        public TQuery Create<TQuery>() where TQuery : IQuery
        {
            return this.iocContainer.Get<TQuery>();
        }

        public TQuery Create<TQuery>(Action<TQuery> builder) where TQuery : IQuery
        {
            var command = this.Create<TQuery>();

            if (builder != null)
            {
                builder(command);
            }

            return command;
        }

        public async Task<IQueryResult<TResult>> ExecuteInternal<TQuery, TResult>(TQuery command,
            Func<TQuery, TResult> resultAccessor) where TQuery : IQuery
        {
            var result = new QueryExecutionResult<TResult>(teclyn);

            this.CheckContextInternal(command, result);
            this.CheckParametersInternal(command, result);

            if (!result.Errors.Any())
            {
                // execute
                await command.Execute(result);

                // get result
                result.Result = resultAccessor(command);

                result.SetSuccess();
            }

            return result;
        }

        public QueryExecutionResult CheckContext(IQuery query)
        {
            var result = new QueryExecutionResult(this.teclyn);

            this.CheckContextInternal(query, result);

            return result;
        }

        public QueryExecutionResult CheckContextAndParameters(IQuery query)
        {
            var result = new QueryExecutionResult(this.teclyn);

            this.CheckContextInternal(query, result);
            this.CheckParametersInternal(query, result);

            return result;
        }

        private void CheckParametersInternal(IQuery query, QueryExecutionResult result)
        {
            result.ParametersAreValid = query.CheckParameters(result);
        }

        private void CheckContextInternal(IQuery query, QueryExecutionResult result)
        {
            result.ContextIsValid = query.CheckContext(context, result);
        }
    }
}
