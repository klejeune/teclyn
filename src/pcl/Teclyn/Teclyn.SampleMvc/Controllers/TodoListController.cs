using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StructureMap.Attributes;
using Teclyn.Core.Storage;
using Teclyn.SampleCore.TodoLists.Models;

namespace Teclyn.SampleMvc.Controllers
{
    public class TodoListController : Controller
    {
        [SetterProperty]
        public IRepository<ITodoList> TodoLists { get; set; }

        // GET: TodoList
        public ActionResult Index(string id)
        {
            var todoList = this.TodoLists.GetById(id);

            return View(todoList);
        }
    }
}