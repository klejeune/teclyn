using System.Web.Mvc;
using Teclyn.AspNetMvc.Mvc.Models;
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
        public ActionResult Execute(ICommand command)
        {
            var result = this.CommandService.Execute(command);
            var userFriendlyResult = result.ToUserFriendly();

            return Json(userFriendlyResult);
        }

        [HttpPost]
        public ActionResult ExecutePost(ICommand command, string returnUrl)
        {
            this.CommandService.Execute(command);
            
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = this.Request.UrlReferrer?.ToString();
            }

            return Redirect(returnUrl);
        }
    }
}