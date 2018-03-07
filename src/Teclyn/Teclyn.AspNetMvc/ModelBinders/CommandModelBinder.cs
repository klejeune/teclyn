using System;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using System.Windows.Input;
using Teclyn.AspNetMvc.Commands;
using Teclyn.Core;
using Teclyn.Core.Api;
using Teclyn.Core.Commands;
using Teclyn.Core.Ioc;
using IDependencyResolver = Teclyn.Core.Ioc.IDependencyResolver;

namespace Teclyn.AspNetMvc.ModelBinders
{
    public class CommandModelBinder : DefaultModelBinder
    {
        private readonly CommandRenderer _commandRenderer;
        private readonly CommandService _commandService;

        public CommandModelBinder(CommandRenderer commandRenderer, CommandService commandService)
        {
            this._commandRenderer = commandRenderer;
            this._commandService = commandService;
        }

        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            var commandType = this._commandRenderer.GetCommandType(controllerContext.HttpContext);
            
            var item = Activator.CreateInstance(commandType);
            bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => item, commandType);

            return item;
        }
    }
}