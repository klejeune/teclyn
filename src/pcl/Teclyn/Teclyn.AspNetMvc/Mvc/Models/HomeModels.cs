using System.Collections.Generic;
using Teclyn.Core.Api;

namespace Teclyn.AspNetMvc.Mvc.Models
{
    public class HomeInfoModel
    {
        public string TeclynVersion { get; set; }
        public IEnumerable<AggregateInfo> Aggregates { get; set; }
    }
}