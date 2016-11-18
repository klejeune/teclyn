using System.Collections.Generic;
using Teclyn.Core.Api;
using Teclyn.Core.Commands;
using Teclyn.Core.Metadata;

namespace Teclyn.AspNetMvc.Mvc.Models
{
    public class HomeInfoModel
    {
        public string TeclynVersion { get; set; }
        public AggregateInfoModel[] Aggregates { get; set; }
        public IEnumerable<CommandInfo> Commands { get; set; }
        public IEnumerable<EventInfo> Events { get; set; }
    }

    public class AggregateInfoModel
    {
        public string AggregateType { get; set; }
        public string ImplementationType { get; set; }
    }
}