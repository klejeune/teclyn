using System;
using System.Web.Mvc;
using System.Windows.Input;
using Teclyn.AspNetMvc.Commands;
using Teclyn.Core;
using Teclyn.Core.Commands;
using Teclyn.Core.Ioc;

namespace Teclyn.AspNetMvc.ModelBinders
{
    public class CommandModelBinder : DefaultModelBinder
    {
        private TeclynApi teclyn;

        public CommandModelBinder(TeclynApi teclyn)
        {
            this.teclyn = teclyn;
        }

        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            var commandRenderer = teclyn.Get<CommandRenderer>();
            var commandService = teclyn.Get<CommandService>();
            var commandType = commandRenderer.GetCommandType(controllerContext.HttpContext);

            if (!commandService.IsRemote(commandType))
            {
                throw new MvcCommandException($"The command {commandType} cannot be called remotely by the html/javascript side. Add the [Remote] attribute or call it from the server side.");
            }

            return base.CreateModel(controllerContext, bindingContext, commandType);
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var commandRenderer = teclyn.Get<CommandRenderer>();
            var commandType = commandRenderer.GetCommandType(controllerContext.HttpContext);

            var context = new ModelBindingContext(bindingContext);
            var item = Activator.CreateInstance(commandType);

            Func<object> modelAccessor = () => item;
            context.ModelMetadata = new ModelMetadata(new DataAnnotationsModelMetadataProvider(), bindingContext.ModelMetadata.ContainerType, modelAccessor, item.GetType(), bindingContext.ModelName);

            var model = base.BindModel(controllerContext, context);

            if (model != null)
            {
                this.teclyn.Get<IIocContainer>().Inject(model);
            }

            return model;
        }
    }
}