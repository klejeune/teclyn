using System.Reflection;
using System.Web.Mvc;

namespace Teclyn.AspNetMvc.Commands.Renderers
{
    public class DateTimeRenderer : ICommandPropertyRenderer<int>
    {
        public bool CanRender(PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType == typeof(int);
        }

        public MvcHtmlString Render(string name, PropertyInfo propertyInfo, int value, string @class)
        {
            return new MvcHtmlString($"<input type='date' name='{name}' value='{value}' class='{@class}' />");
        }

        public MvcHtmlString RenderHidden(string name, PropertyInfo propertyInfo, int value)
        {
            return new MvcHtmlString($"<input type='hidden' name='{name}' value='{value}' />");
        }
    }
}