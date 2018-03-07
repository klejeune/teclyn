using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Teclyn.Core.Basic;
using Teclyn.Core.Commands;
using Teclyn.Core.Domains;
using Teclyn.Core.Queries;
using Teclyn.Core.Storage;

namespace Teclyn.Core.Api
{
    public class TeclynDomainApi<TDomain> : ITeclynDomainApi<TDomain> where TDomain : IDomain
    {
        private readonly List<CommandInfo> _commands = new List<CommandInfo>();
        private readonly List<QueryInfo> _queries = new List<QueryInfo>();
        private readonly List<AggregateInfo> _aggregates = new List<AggregateInfo>();
        
        public ITeclynDomainApi<TDomain> AddCommand<TCommand, TCommandHandler>() where TCommand : ICommand where TCommandHandler : ICommandHandler<TCommand>
        {
            this._commands.Add(new CommandInfo(
                this.GetCommandId<TCommand>(),
                this.GetCommandName<TCommand>(),
                typeof(TCommand),
                typeof(ICommandHandler<TCommand>),
                typeof(TCommandHandler),
                this.GetCommandParameters<TCommand>()));

            return this;
        }

        public ITeclynDomainApi<TDomain> AddQuery<TQuery, TQueryHandler, TResult>() where TQuery : IQuery<TResult> where TQueryHandler : IQueryHandler<TQuery, TResult>
        {
            this._queries.Add(new QueryInfo(
                this.GetQueryId<TQuery, TResult>(),
                this.GetQueryName<TQuery, TResult>(),
                typeof(TQuery),
                typeof(IQueryHandler<TQuery, TResult>),
                typeof(TQueryHandler),
                typeof(TResult),
                this.GetQueryParameters<TQuery, TResult>()));

            return this;
        }

        public ITeclynDomainApi<TDomain> AddAggregate<TAggregate>(Func<ITeclynAggregateApi<TDomain, TAggregate>, ITeclynAggregateApi<TDomain, TAggregate>> _) where TAggregate : IAggregate
        {
            this._aggregates.Add(new AggregateInfo(
                this.GetAggregateId<TAggregate>(),
                this.GetAggregateId<TAggregate>(),
                typeof(TAggregate),
                typeof(TAggregate),
                typeof(IRepository<TAggregate>),
                typeof(Repository<TAggregate>),
                typeof(IRepositoryProvider<TAggregate>),
                typeof(InMemoryRepositoryProvider<TAggregate>),
                this.GetAggregateId<TAggregate>(),
                null,
                null));

            return this;
        }

        public IEnumerable<AggregateInfo> Aggregates => this._aggregates;

        public IEnumerable<CommandInfo> Commands => this._commands;

        public IEnumerable<QueryInfo> Queries => this._queries;

        private string GetCommandId<TCommand>() where TCommand : ICommand
        {
            return this.GetCommandName<TCommand>();
        }

        private string GetQueryId<TQuery, TResult>() where TQuery : IQuery<TResult>
        {
            return this.GetQueryName<TQuery, TResult>();
        }

        private string GetQueryName<TQuery, TResult>() where TQuery : IQuery<TResult>
        {
            var queryTypeName = typeof(TQuery).Name;

            if (queryTypeName.EndsWith("Query"))
            {
                queryTypeName = queryTypeName.Substring(0, queryTypeName.Length - 5);
            }

            return queryTypeName;
        }

        private string GetCommandName<TCommand>() where TCommand : ICommand
        {
            var commandTypeName = typeof(TCommand).Name;

            if (commandTypeName.EndsWith("Command"))
            {
                commandTypeName = commandTypeName.Substring(0, commandTypeName.Length - 7);
            }

            return commandTypeName;
        }

        private IEnumerable<CommandParameterInfo> GetCommandParameters<TCommand>() where TCommand : ICommand
        {
            return typeof(TCommand).GetProperties().Select(p => new CommandParameterInfo(p.Name));
        }

        private IDictionary<string, Type> GetQueryParameters<TQuery, TResult>() where TQuery : IQuery<TResult>
        {
            return typeof(TQuery).GetProperties().ToDictionary(p => p.Name, p => p.PropertyType);
        }

        private string GetAggregateId<TAggregate>() where TAggregate : IAggregate
        {
            var id = typeof(TAggregate).Name;

            if (id.Length >= 3 && id[0] == 'I')
            {
                id = id.Substring(1);
            }

            return id;
        }
    }
}