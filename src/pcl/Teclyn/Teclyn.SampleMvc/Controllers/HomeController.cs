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
        [Inject]
        public IRepository<ITodoList> TodoLists { get; set; }

        public HomeController(IRepository<ITodoList> repository)
        {
            //this.TodoLists = repository;
        }

        public ActionResult Index()
        {
            var model = new HomeModel
            {
                TodoLists = this.TodoLists.ToList()
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