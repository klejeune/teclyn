using System.Reflection;
using System.Web.Mvc;

namespace Teclyn.AspNetMvc.Commands.Renderers
{
    public class StringInputCommandPropertyRenderer : ICommandPropertyRenderer<string>
    {
        public MvcHtmlString Render(string name, PropertyInfo propertyInfo, string value, string @class)
        {
            return new MvcHtmlString($"<input type='text' name='{name}' value='{value}' class='{@class}' />");
        }

        public MvcHtmlString RenderHidden(string name, PropertyInfo propertyInfo, string value)
        {
            return new MvcHtmlString($"<input type='hidden' name='{name}' value='{value}' />");
        }

        public bool CanRender(PropertyInfo propertyInfo)
        {
            return true;
        }
    }
}