
// ReSharper disable once CheckNamespace

using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using JetBrains.Annotations;
using Teclyn.AspNetMvc;
using Teclyn.AspNetMvc.Commands;
using Teclyn.Core;
using Teclyn.Core.Commands;
using Teclyn.Core.Domains;
using Teclyn.Core.Ioc;

public static class MvcHtmlExtensions
{
    private static CommandRenderer CommandRenderer => HttpContext.Current.Application.GetTeclyn().Get<CommandRenderer>();

    public static MvcHtmlString ActionLink(this HtmlHelper helper, IDisplayable item, [AspMvcAction]string actionName, [AspMvcController]string controllerName)
    {
        return System.Web.Mvc.Html.LinkExtensions.ActionLink(helper, item.Name, actionName, controllerName, new {Id = item.Id}, null);
    }

    public static MvcHtmlString ActionLink(this HtmlHelper helper, IDisplayable item, [AspMvcAction]string actionName)
    {
        return System.Web.Mvc.Html.LinkExtensions.ActionLink(helper, item.Name, actionName, new { Id = item.Id }, null);
    }

    public static CommandButton CommandButton<TCommand>(this HtmlHelper helper, Action<TCommand> builder, string @class = "", object htmlAttributes = null)
        where TCommand : IBaseCommand
    {
        var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes).SafeCast<IDictionary<string, object>>();

        return CommandRenderer.RenderCommandButton(helper.ViewContext.Writer, builder, false, @class, attributes);
    }

    public static MvcHtmlString CommandButton<TCommand>(this HtmlHelper helper, string label, Action<TCommand> builder, string @class = "", object htmlAttributes = null)
        where TCommand : IBaseCommand
    {
        var writer = new StringWriter();

        var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes).SafeCast<IDictionary<string, object>>();

        using (CommandRenderer.RenderCommandButton(writer, builder, false, @class, attributes))
        {
            writer.Write(label);
        }

        return new MvcHtmlString(writer.ToString());
    }

    public static CommandForm<TCommand> CommandForm<TCommand>(this HtmlHelper helper, bool reload = false, string @class = null, object htmlAttributes = null, string returnUrl = null) where TCommand : IBaseCommand
    {
        return CommandRenderer.RenderCommandForm<TCommand>(helper, reload, @class, htmlAttributes, returnUrl);
    }
}
