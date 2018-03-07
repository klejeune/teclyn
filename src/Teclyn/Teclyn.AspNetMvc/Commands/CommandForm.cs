using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Teclyn.AspNetMvc.Commands.Renderers;
using Teclyn.Core;
using Teclyn.Core.Commands;
using Teclyn.Core.Tools;

namespace Teclyn.AspNetMvc.Commands
{
    public class CommandForm<TCommand> : IDisposable where TCommand : ICommand
    {
        private TextWriter writer;
        private CommandRenderer rendererService;

        public CommandForm(CommandRenderer rendererService, TCommand command, HtmlHelper helper, bool reload, string @class, object htmlAttributes, string returnUrl)
        {
            this.Command = command;
            this.writer = helper.ViewContext.Writer;
            this.rendererService = rendererService;
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            
            var classes = string.IsNullOrWhiteSpace(@class) ? "command-form" : "command-form " + @class;

            writer.Write($"<form class='{classes}' method='POST' action='{urlHelper.Action("ExecutePost", "Command")}' data-reload='{reload.ToString().ToLowerInvariant()}'>");
            writer.Write($"<input type='hidden' name='{rendererService.GetCommandTypeAttributeName()}' value='{typeof(TCommand).AssemblyQualifiedName}' />");
            writer.Write($"<input type='hidden' name='ReturnUrl' value='{returnUrl}' />");
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
            var propertyInfo = GetProperty(property);

            var renderer = this.rendererService.GetRenderer<TResult>(propertyInfo);
            var propertyName = this.rendererService.GetPropertyName(propertyInfo);

            return renderer.Render(propertyName, propertyInfo, value, @class);
        }

        public static PropertyInfo GetProperty<TType, TValue>(Expression<Func<TType, TValue>> selector)
        {
            Expression body = selector;
            if (body is LambdaExpression)
            {
                body = ((LambdaExpression)body).Body;
            }
            switch (body.NodeType)
            {
                case ExpressionType.MemberAccess:
                    var propertyInfo = (PropertyInfo)((MemberExpression)body).Member;

                    return GetOverridenPropertyInfo(propertyInfo, typeof(TType));
                default:
                    throw new InvalidOperationException();
            }
        }

        private static PropertyInfo GetOverridenPropertyInfo(PropertyInfo property, Type childType)
        {
            if (property.DeclaringType == childType)
            {
                return property;
            }

            if (property.DeclaringType != null && property.DeclaringType.GetTypeInfo().IsInterface && childType.GetTypeInfo().ImplementedInterfaces.Contains(property.DeclaringType))
            {
                InterfaceMapping map = childType.GetTypeInfo().GetRuntimeInterfaceMap(property.DeclaringType);
                MethodInfo getMethod = property.GetMethod;
                for (int i = 0; i < map.InterfaceMethods.Length; i++)
                {
                    if (map.InterfaceMethods[i] == getMethod)
                    {
                        MethodInfo target = map.TargetMethods[i];
                        return childType.GetTypeInfo().GetProperties().Where(p => p.CanRead && !p.GetMethod.IsStatic)
                            .FirstOrDefault(myProperty => myProperty.GetMethod == target);
                    }
                }

                throw new TeclynException($"Unable to get the implementation of property {property.Name} of interface {property.DeclaringType}.");
            }
            else if (property.DeclaringType != null && property.DeclaringType.GetTypeInfo().IsClass)
            {
                return property;
            }

            throw new TeclynException($"Unable to get the implementation of property {property.Name} of interface {property.DeclaringType}.");
        }

        public MvcHtmlString Hidden<TResult>(Expression<Func<TCommand, TResult>> property, TResult value, string @class = null)
        {
            var propertyInfo = ReflectionTools.Instance<TCommand>.Property(property);

            var renderer = this.rendererService.GetRenderer<TResult>(propertyInfo);
            var propertyName = this.rendererService.GetPropertyName(propertyInfo);

            return renderer.RenderHidden(propertyName, propertyInfo, value);
        }

        public MvcHtmlString Submit(string label, string @class = null)
        {
            return new MvcHtmlString($"<button class='{@class}' type='submit'>{HttpUtility.HtmlEncode(label)}</button>");
        }

        public TCommand Command { get; private set; }
    }
}