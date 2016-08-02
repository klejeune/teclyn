using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StructureMap.Attributes;
using Teclyn.AspNetMvc.Models;
using Teclyn.Core.Storage;
using Teclyn.SampleCore.TodoLists.Models;

namespace Teclyn.SampleMvc.Controllers
{
    public class TodoListController : Controller
    {
        [SetterProperty]
        public IRepository<ITodoList> TodoLists { get; set; }

        // GET: TodoList
        public ActionResult Index(string id, int? s)
        {
            var todoList = this.TodoLists.GetByIdOrNull(id);

            var a = this.TodoLists.OrderByDescending(c => c.CreationDate);

            if (todoList == null)
            {
                var todoLists = new ListModel<ITodoList>(
                    this.TodoLists.OrderByDescending(c => c.CreationDate),
                    index => this.Url.Action("Index", new { s = index }),
                    s.GetValueOrDefault(),
                    20);

                return View(todoLists);
            }
            else
            {
                return View("Get", todoList);
            }
        }
    }
}