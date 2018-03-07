using System.Reflection;
using System.Web.Mvc;
using Teclyn.AspNetMvc.Commands.Renderers;
using Teclyn.Core.Commands.Semantics;

namespace Teclyn.SampleMvc.Renderers
{
    public class TinyMceRenderer : ICommandPropertyRenderer<string>
    {
        public bool CanRender(PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType == typeof(string) && propertyInfo.GetCustomAttribute<RichTextAttribute>() != null;
        }

        public MvcHtmlString Render(string name, PropertyInfo propertyInfo, string value, string @class)
        {
            return new MvcHtmlString($"<input type='text' name='{name}' value='{value}' class='rich-text-editor {@class}' />");
        }

        public MvcHtmlString RenderHidden(string name, PropertyInfo propertyInfo, string value)
        {
            return new MvcHtmlString($"<input type='hidden' name='{name}' value='{value}' />");
        }
    }
}   