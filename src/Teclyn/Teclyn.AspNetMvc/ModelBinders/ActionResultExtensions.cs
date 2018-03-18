using System.Web.Mvc;

namespace Teclyn.AspNetMvc.ModelBinders
{
    public static class ActionResultExtensions
    {
        public static ActionResult Structured<T>(this Controller controller, T data)
        {
            return new StructuredDataResult<T>(data, false);
        }
    }
}