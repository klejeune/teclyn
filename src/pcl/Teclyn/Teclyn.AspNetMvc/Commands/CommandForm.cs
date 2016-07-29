using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Teclyn.Core.Commands;
using Teclyn.Core.Tools;

namespace Teclyn.AspNetMvc.Commands
{
    public class CommandForm<TCommand> : IDisposable where TCommand : ICommand
    {
        private TextWriter writer;
        private CommandRenderer rendererService;

        public CommandForm(CommandRenderer rendererService, HtmlHelper helper, bool reload = false, string @class = "", object htmlAttributes = null)
        {
            this.writer = helper.ViewContext.Writer;
            this.rendererService = rendererService;
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            
            var classes = string.IsNullOrWhiteSpace(@class) ? "command-form" : "command-form " + @class;

            writer.Write($"<form class='{classes}' method='POST' action='{urlHelper.Action("ExecutePost", "Command", new { Area = "Teclyn" })}'>");
            writer.Write($"<input type='hidden' name='{rendererService.GetCommandTypeAttributeName()}' value='{typeof(TCommand).AssemblyQualifiedName}' />");
        }
        
        public void Dispose()
        {
            writer.Write("</form>");
        }

        public MvcHtmlString Property<TResult>(Expression<Func<TCommand, TResult>> property, string @class = null)
        {
            return Property(property, default(TResult), @class);
        }

        public MvcHtmlString Property<TResult>(Expression<Func<TCommand, TResult>> property, TResult value, string @class = null)
        {
            var propertyInfo = ReflectionTools.Instance<TCommand>.Property(property);

            var renderer = this.rendererService.GetRenderer<TResult>(propertyInfo);
            var propertyName = this.rendererService.GetPropertyName(propertyInfo);

            return renderer.Render(propertyName, propertyInfo, value, @class);
        }

        public MvcHtmlString Submit(string label, string @class = null)
        {
            return new MvcHtmlString($"<button class='{@class}' type='submit'>{HttpUtility.HtmlEncode(label)}</button>");
        }
    }
}