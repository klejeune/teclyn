
// ReSharper disable once CheckNamespace

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JetBrains.Annotations;
using Teclyn.AspNetMvc;
using Teclyn.AspNetMvc.Commands;
using Teclyn.Core;
using Teclyn.Core.Commands;
using Teclyn.Core.Commands.Properties;
using Teclyn.Core.Domains;
using Teclyn.Core.Dummies;
using Teclyn.Core.Ioc;
using Teclyn.Core.Tools;

public static class MvcHtmlExtensions
{
    private static CommandRenderer CommandRenderer => HttpContext.Current.Application.GetTeclyn().Get<CommandRenderer>();
    private static CommandService CommandService => HttpContext.Current.Application.GetTeclyn().Get<CommandService>();

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

    public static MvcHtmlString CommandProperty<TCommand, TAggregate, TProperty>(this HtmlHelper helper, TAggregate aggregate, bool reload = false, string @class = null, object htmlAttributes = null) 
        where TCommand : IPropertyCommand<TAggregate, TProperty>
        where TAggregate : IAggregate
    {
        var writer = helper.ViewContext.Writer;

        using (var form = helper.CommandForm<TCommand>(reload, @class, htmlAttributes, null))
        {
            writer.Write(form.Property(command => command.NewValue, form.Command.PropertyAccessor(aggregate)).ToHtmlString());
            writer.Write(form.Hidden(command => command.AggregateId, aggregate.Id));
            writer.Write(form.Submit("OK"));
        }

        return MvcHtmlString.Empty;
    }

    public static MvcHtmlString CommandProperty<TCommand>(this HtmlHelper helper,
        object aggregate, bool reload = false, string @class = null, object htmlAttributes = null)
        where TCommand : IPropertyCommand
    {
        var typedPropertyCommandType = typeof(TCommand).GetInterfaces().Single(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IPropertyCommand<,>));
        var aggregateType = typedPropertyCommandType.GetGenericArguments()[0];
        var propertyType = typedPropertyCommandType.GetGenericArguments()[1];

        var typedMethod = ReflectionTools.Static.Method(() => CommandProperty<DummyPropertyCommand, IDummyAggregate, string>(helper, null, false, null, null))
            .GetGenericMethodDefinition()
            .MakeGenericMethod(typeof(TCommand), aggregateType, propertyType);

        return (MvcHtmlString) typedMethod.Invoke(null, new object[] {helper, aggregate, reload, @class, htmlAttributes});
    }
}
