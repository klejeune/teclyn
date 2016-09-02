using System.Reflection;
using System.Web.Mvc;

namespace Teclyn.AspNetMvc.Commands.Renderers
{
    public interface ICommandPropertyRenderer
    {
        bool CanRender(PropertyInfo propertyInfo);
    }

    public interface ICommandPropertyRenderer<in TProperty> : ICommandPropertyRenderer
    {
        MvcHtmlString Render(string name, PropertyInfo propertyInfo, TProperty value, string @class);
        MvcHtmlString RenderHidden(string name, PropertyInfo propertyInfo, TProperty value);
    }
}