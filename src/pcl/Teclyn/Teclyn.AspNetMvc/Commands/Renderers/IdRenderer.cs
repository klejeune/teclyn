using System.Reflection;
using System.Web.Mvc;
using Teclyn.Core.Commands;

namespace Teclyn.AspNetMvc.Commands.Renderers
{
    public class IdRenderer : ICommandPropertyRenderer<Id>
    {
        public MvcHtmlString Render(string name, PropertyInfo propertyInfo, Id value, string @class)
        {
            return new MvcHtmlString($"<input type='text' name='{name}' value='{value.Value}' class='{@class}' />");
        }

        public MvcHtmlString RenderHidden(string name, PropertyInfo propertyInfo, Id value)
        {
            return new MvcHtmlString($"<input type='hidden' name='{name}' value='{value.Value}' />");
        }

        public bool CanRender(PropertyInfo propertyInfo)
        {
            return typeof(Id).IsAssignableFrom(propertyInfo.PropertyType);
        }
    }
}