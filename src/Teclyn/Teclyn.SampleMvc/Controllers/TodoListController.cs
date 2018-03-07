using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using StructureMap.Attributes;
using Teclyn.AspNetMvc.Models;
using Teclyn.Core.Ioc;
using Teclyn.Core.Storage;
using Teclyn.SampleCore.TodoLists.Models;
using Teclyn.SampleCore.Todos.Models;
using Teclyn.SampleMvc.Models;

namespace Teclyn.SampleMvc.Controllers
{
    public class TodoListController : Controller
    {
        private IRepository<ITodoList> _todoLists;

        public TodoListController(IRepository<ITodoList> todoLists, IRepository<ITodo> todos)
        {
            this._todoLists = todoLists;
            this._todos = todos;
        }

        private IRepository<ITodo> _todos;

        // GET: TodoList
        public async Task<ActionResult> Index(string id, int? s)
        {
            var todoList = await this._todoLists.GetByIdOrNull(id);

            if (todoList == null)
            {
                var todoLists = new ListModel<ITodoList>(
                    this._todoLists.OrderByDescending(c => c.CreationDate),
                    index => this.Url.Action("Index", new { s = index }),
                    s.GetValueOrDefault(),
                    20);

                return View(todoLists);
            }
            else
            {
                var model = new TodoListModel
                {
                    TodoList = todoList,
                    Todos = this._todos.Where(t => t.TodoList.Id == todoList.Id),
                };

                return View("Get", model);
            }
        }
    }
}