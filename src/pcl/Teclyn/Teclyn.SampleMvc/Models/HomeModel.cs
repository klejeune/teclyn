using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Teclyn.SampleCore.TodoLists.Models;

namespace Teclyn.SampleMvc.Models
{
    public class HomeModel
    {
        public IEnumerable<ITodoList> TodoLists { get; set; }
    }
}