using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Teclyn.Core.Ioc;
using Teclyn.Core.Storage;
using Teclyn.SampleCore.Todos.Models;

namespace Teclyn.SampleMvc.Controllers
{
    public class TodoController : Controller
    {
        [Inject]
        public IRepository<ITodo> Todos { get; set; }

        public async Task<ActionResult> Index(string id)
        {
            var todo = await this.Todos.GetById(id);

            return View(todo);
        }
    }
}