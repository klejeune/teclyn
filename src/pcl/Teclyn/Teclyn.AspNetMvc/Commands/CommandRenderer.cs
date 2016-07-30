using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Teclyn.AspNetMvc.Commands.Renderers;
using Teclyn.Core.Commands;
using Teclyn.Core.Ioc;
using Teclyn.Core.Tools;

namespace Teclyn.AspNetMvc.Commands
{
    public class CommandRenderer
    {
        private static readonly string commandTypeHtmlAttribute = "CommandType";

        [Inject]
        public IIocContainer IocContainer { get; set; }

        [Inject]
        public CommandService CommandService { get; set; }

        [Inject]
        public JsonSerializer JsonSerializer { get; set; }

        [Inject]
        public Base64Serializer Base64Serializer { get; set; }

        [Inject]
        public CommandPropertyRendererFactory CommandPropertyRendererFactory { get; set; }
        
        public CommandButton RenderCommandButton<TCommand>(TextWriter writer, Action<TCommand> builder, bool reload, string @class, IDictionary<string, object> htmlAttributes) where TCommand : ICommand
        {
            var command = this.IocContainer.Get<TCommand>();

            if (!this.CommandService.IsRemote<TCommand>())
            {
                throw new MvcCommandRenderingException($"The command {typeof(TCommand).Name} cannot be called remotely by the html/javascript side. Add the [Remote] attribute or call it from the server side.");
            }

            if (builder != null)
            {
                builder(command);
            }

            var check = this.CommandService.CheckContextAndParameters(command);

            if (check.Errors.Any())
            {
                throw new MvcCommandRenderingException($"The command {typeof(TCommand).Name} cannot be executed:\n" 
                    + string.Join("\n", check.Errors));
            }

            var parameters = this.CommandService.Serialize(command);
            var serializedCommand = this.Base64Serializer.Serialize(this.JsonSerializer.Serialize(parameters));

            htmlAttributes["data-" + commandTypeHtmlAttribute] = command.GetType().AssemblyQualifiedName;

            return new CommandButton(writer, serializedCommand, reload, @class, htmlAttributes);
        }

        public CommandForm<TCommand> RenderCommandForm<TCommand>(HtmlHelper helper, bool reload, string @class, object htmlAttributes, string returnUrl) where TCommand : ICommand
        {
            var command = this.IocContainer.Get<TCommand>();

            if (!this.CommandService.IsRemote<TCommand>())
            {
                throw new MvcCommandRenderingException($"The command {typeof(TCommand).Name} cannot be called remotely by the html/javascript side. Add the [Remote] attribute or call it from the server side.");
            }
            
            var check = this.CommandService.CheckContext(command);

            if (check.Errors.Any())
            {
                throw new MvcCommandRenderingException($"The command {typeof(TCommand).Name} cannot be executed:\n" + string.Join("\n", check.Errors));
            }

            return new CommandForm<TCommand>(this, helper, reload, @class, htmlAttributes, returnUrl);
        }

        public string GetPropertyName(PropertyInfo propertyInfo)
        {
            return "Command." + propertyInfo.Name;
        }

        public ICommandPropertyRenderer<TProperty> GetRenderer<TProperty>(PropertyInfo property)
        {
            return this.CommandPropertyRendererFactory.GetRenderer<TProperty>(property);
        }

        public Type GetCommandType(HttpContextBase context)
        {
            var commandTypeAsString = context.Request.Params[commandTypeHtmlAttribute];

            var type = Type.GetType(commandTypeAsString);

            return type;
        }

        public string GetCommandTypeAttributeName()
        {
            return commandTypeHtmlAttribute;
        }
    }
}