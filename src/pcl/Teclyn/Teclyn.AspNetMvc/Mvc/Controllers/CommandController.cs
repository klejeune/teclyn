using System.Web.Mvc;
using Teclyn.Core;
using Teclyn.Core.Commands;
using Teclyn.Core.Ioc;

namespace Teclyn.AspNetMvc.Mvc.Controllers
{
    public class CommandController : Controller
    {
        [Inject]
        public CommandService CommandService { get; set; }

        public ActionResult Index()
        {
            return this.Content("Index OK");
        }

        [HttpPost]
        public ActionResult ExecutePost(ICommand test)
        {
            var result = this.CommandService.Execute(test);
            var userFriendlyResult = result.ToUserFriendly();
            
            return Json(userFriendlyResult);
        }
    }
}