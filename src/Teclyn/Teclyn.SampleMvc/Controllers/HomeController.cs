using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StructureMap.Attributes;
using StructureMap.Building;
using Teclyn.Core.Ioc;
using Teclyn.Core.Storage;
using Teclyn.SampleCore.TodoLists.Models;
using Teclyn.SampleMvc.Models;

namespace Teclyn.SampleMvc.Controllers
{
    public class HomeController : Controller
    {
        private IRepository<ITodoList> _todoLists;

        public HomeController(IRepository<ITodoList> repository, IRepository<ITodoList> todoLists)
        {
            this._todoLists = todoLists;
            //this.TodoLists = repository;
        }

        public ActionResult Index()
        {
            var model = new HomeModel
            {
                TodoLists = this._todoLists.ToList()
            };


            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}