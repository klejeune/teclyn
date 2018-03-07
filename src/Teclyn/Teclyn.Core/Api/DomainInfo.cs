using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Teclyn.Core.Domains;

namespace Teclyn.Core.Api
{
    public class DomainInfo
    {
        public DomainInfo(string id, string name, IEnumerable<CommandInfo> commands, IEnumerable<QueryInfo> queries, IEnumerable<AggregateInfo> aggregates)
        {
            this.Id = id;
            this.Name = name;
            this.Commands = commands;
            this.Queries = queries;
            this.Aggregates = aggregates;
        }

        public string Id { get; }
        public string Name { get; }
        public IEnumerable<CommandInfo> Commands { get; }
        public IEnumerable<QueryInfo> Queries { get; }
        public IEnumerable<AggregateInfo> Aggregates { get; }
    }
}