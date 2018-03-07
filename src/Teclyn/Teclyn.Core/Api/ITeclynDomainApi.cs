using System;
using System.Collections.Generic;
using Teclyn.Core.Commands;
using Teclyn.Core.Domains;
using Teclyn.Core.Queries;

namespace Teclyn.Core.Api
{
    public interface ITeclynDomainApi<TDomain>
        where TDomain : IDomain
    {
        ITeclynDomainApi<TDomain> AddCommand<TCommand, TCommandHandler>()
            where TCommand : ICommand
            where TCommandHandler : ICommandHandler<TCommand>;

        ITeclynDomainApi<TDomain> AddQuery<TQuery, TQueryHandler, TResult>()
            where TQuery : IQuery<TResult>
            where TQueryHandler : IQueryHandler<TQuery, TResult>;

        ITeclynDomainApi<TDomain> AddAggregate<TAggregate>(
            Func<ITeclynAggregateApi<TDomain, TAggregate>, ITeclynAggregateApi<TDomain, TAggregate>> _)
            where TAggregate : IAggregate;
    }
}