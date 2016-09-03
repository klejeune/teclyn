using System.Web;
using System.Web.Mvc;
using Teclyn.Core.Security.Context;

namespace Teclyn.AspNetMvc.Mvc.Security
{
    public class OnlyAdminFilter : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var context = HttpContext.Current.Application.GetTeclyn().Get<ITeclynContext>();

            if (!context.CurrentUser.IsAdmin)
            {
                filterContext.Result = new HttpUnauthorizedResult("This URL can only be accessed by administrators.");
            }
            else
            {
                base.OnActionExecuting(filterContext);
            }
        }
    }
}