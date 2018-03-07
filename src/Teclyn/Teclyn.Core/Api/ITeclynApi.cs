using System;
using System.Collections.Generic;
using Teclyn.Core.Commands;
using Teclyn.Core.Domains;

namespace Teclyn.Core.Api
{
    public interface ITeclynApi
    {
        ITeclynApiConfiguration Configuration { get; }
        ITeclynApi AddDomain<TDomain>(Func<ITeclynDomainApi<TDomain>, ITeclynDomainApi<TDomain>> _)
            where TDomain : IDomain;

        IEnumerable<DomainInfo> Domains { get; }

        AggregateInfo GetAggregate<TAggregate>() where TAggregate : IAggregate;
        CommandInfo GetCommand<TCommand>() where TCommand : ICommand;
        CommandInfo GetCommand(string domain, string commandId);
        QueryInfo GetQuery(string domainId, string queryId);
        DomainInfo GetDomain(string domainId);
    }
}