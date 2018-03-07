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
using IDependencyResolver = Teclyn.Core.Ioc.IDependencyResolver;

namespace Teclyn.AspNetMvc.Commands
{
    public class CommandRenderer
    {
        private static readonly string commandTypeHtmlAttribute = "CommandType";
        
        public IDependencyResolver DependencyResolver { get; set; }
        
        public CommandService CommandService { get; set; }
        
        public JsonSerializer JsonSerializer { get; set; }
        
        public Base64Serializer Base64Serializer { get; set; }
        
        public CommandPropertyRendererFactory CommandPropertyRendererFactory { get; set; }
        
        public CommandButton RenderCommandButton<TCommand>(TextWriter writer, Action<TCommand> builder, bool reload, string @class, IDictionary<string, object> htmlAttributes) where TCommand : ICommand
        {
            var command = this.DependencyResolver.Get<TCommand>();

            if (builder != null)
            {
                builder(command);
            }
            
            var parameters = this.CommandService.Serialize(command);
            var serializedCommand = this.Base64Serializer.Serialize(this.JsonSerializer.Serialize(parameters));

            htmlAttributes["data-" + commandTypeHtmlAttribute] = command.GetType().AssemblyQualifiedName;

            return new CommandButton(writer, serializedCommand, reload, @class, htmlAttributes);
        }

        public CommandForm<TCommand> RenderCommandForm<TCommand>(HtmlHelper helper, bool reload, string @class, object htmlAttributes, string returnUrl) where TCommand : ICommand
        {
            var command = this.DependencyResolver.Get<TCommand>();
            
            return new CommandForm<TCommand>(this, command, helper, reload, @class, htmlAttributes, returnUrl);
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
            var commandTypeAsString = context.Request.Unvalidated[commandTypeHtmlAttribute];

            var type = Type.GetType(commandTypeAsString);

            return type;
        }

        public string GetCommandTypeAttributeName()
        {
            return commandTypeHtmlAttribute;
        }
    }
}