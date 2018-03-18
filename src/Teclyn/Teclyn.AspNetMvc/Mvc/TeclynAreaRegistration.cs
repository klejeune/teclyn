using System.Linq;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using Teclyn.AspNetMvc.Mvc.Controllers;
using Teclyn.Core.Tools;

namespace Teclyn.AspNetMvc.Mvc
{
    public class TeclynAreaRegistration : AreaRegistration
    {
        public override void RegisterArea(AreaRegistrationContext context)
        {
            var route = context.MapRoute(
                "Teclyn_Area_Default",
                "Teclyn/{controller}/{action}/{id}",
                new {action = "Index", id = UrlParameter.Optional},
                typeof(CommandController).Namespace.AsArray());

            route.DataTokens["UseNamespaceFallback"] = false;
        }

        public override string AreaName => "Teclyn";
    }
}