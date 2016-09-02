using System.Reflection;
using System.Web.Mvc;

namespace Teclyn.AspNetMvc.Commands.Renderers
{
    public class BooleanRenderer : ICommandPropertyRenderer<bool>
    {
        public bool CanRender(PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType == typeof(bool);
        }

        public MvcHtmlString Render(string name, PropertyInfo propertyInfo, bool value, string @class)
        {
            return new MvcHtmlString($"<input type='checkbox' name='{name}' {(value ? "checked" : string.Empty)} class='{@class}' />");
        }

        public MvcHtmlString RenderHidden(string name, PropertyInfo propertyInfo, bool value)
        {
            return new MvcHtmlString($"<input type='hidden' name='{name}' value='{value.ToString().ToLowerInvariant()}' />");
        }
    }
}