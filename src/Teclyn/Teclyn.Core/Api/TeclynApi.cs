using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using Teclyn.Core.Commands;
using Teclyn.Core.Domains;

namespace Teclyn.Core.Api
{
    public class TeclynApi : ITeclynApi
    {
        public ITeclynApiConfiguration Configuration { get; }
        private readonly List<DomainInfo> _domains = new List<DomainInfo>();

        public TeclynApi(ITeclynApiConfiguration _)
        {
            this.Configuration = _;
        }

        public ITeclynApi AddDomain<TDomain>(Func<ITeclynDomainApi<TDomain>, ITeclynDomainApi<TDomain>> configure) where TDomain : IDomain
        {
            var domainId = this.GetDomainId<TDomain>();
            var domainApi = new TeclynDomainApi<TDomain>();

            configure(domainApi);

            this._domains.Add(new DomainInfo(
                domainId,
                this.GetDomainName<TDomain>(),
                domainApi.Commands,
                domainApi.Queries,
                domainApi.Aggregates));

            return this;
        }

        public IEnumerable<DomainInfo> Domains => this._domains;

        public AggregateInfo GetAggregate<TAggregate>() where TAggregate : IAggregate
        {
            return this._domains
                .SelectMany(d => d.Aggregates)
                .Single(a => a.AggregateType == typeof(TAggregate));
        }

        public CommandInfo GetCommand<TCommand>() where TCommand : ICommand
        {
            return this._domains
                .SelectMany(d => d.Commands)
                .Single(c => c.CommandType == typeof(TCommand));
        }

        public DomainInfo GetDomain(string domainId)
        {
            return this._domains
                .SingleOrDefault(d => d.Id == domainId);
        }

        public CommandInfo GetCommand(string domain, string commandId)
        {
            var lowerCase = commandId.ToLowerInvariant();

            return this._domains
                .Where(d => d.Id == domain)
                .SelectMany(d => d.Commands)
                .SingleOrDefault(c => c.Id.ToLowerInvariant() == lowerCase);
        }

        public QueryInfo GetQuery(string domainId, string queryId)
        {
            var lowerCase = queryId.ToLowerInvariant();

            return this._domains
                .Where(d => d.Id == domainId)
                .SelectMany(d => d.Queries)
                .SingleOrDefault(c => c.Id.ToLowerInvariant() == lowerCase);
        }

        private string GetAttributeProperty<TAttribute>(Type type, Func<TAttribute, string> accessor) where TAttribute : Attribute
        {
            var attribute = (TAttribute) type.GetCustomAttribute(typeof(TAttribute), true);

            return attribute == null ? null : accessor(attribute);
        }

        private string GetDomainName<TDomain>() where TDomain : IDomain
        {
            var domainTypeName = this.GetAttributeProperty<DomainNameAttribute>(typeof(TDomain), attribute => attribute.Name);

            if (string.IsNullOrWhiteSpace(domainTypeName))
            {
                domainTypeName = typeof(TDomain).Name;

                if (domainTypeName.ToLowerInvariant().EndsWith("domain"))
                {
                    domainTypeName = domainTypeName.Substring(0, domainTypeName.Length - 6);
                }
            }

            return domainTypeName;
        }

        private string GetDomainId<TDomain>() where TDomain : IDomain
        {
            return this.GetDomainName<TDomain>();
        }
    }
}