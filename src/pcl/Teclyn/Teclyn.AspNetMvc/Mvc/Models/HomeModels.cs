using System.Collections.Generic;
using Teclyn.Core.Api;

namespace Teclyn.AspNetMvc.Mvc.Models
{
    public class HomeInfoModel
    {
        public string TeclynVersion { get; set; }
        public AggregateInfoModel[] Aggregates { get; set; }
    }

    public class AggregateInfoModel
    {
        public string AggregateType { get; set; }
        public string ImplementationType { get; set; }
    }
}