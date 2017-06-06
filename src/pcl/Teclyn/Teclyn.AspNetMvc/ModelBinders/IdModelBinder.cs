using System;
using System.Web.Mvc;
using Teclyn.AspNetMvc.Commands;
using Teclyn.Core.Commands;
using Teclyn.Core.Ioc;

namespace Teclyn.AspNetMvc.ModelBinders
{
    public class IdModelBinder : DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            var o = base.CreateModel(controllerContext, bindingContext, modelType);

            return o;

            //if ()


            //var commandRenderer = teclyn.Get<CommandRenderer>();
            //var commandService = teclyn.Get<CommandService>();
            //var commandType = commandRenderer.GetCommandType(controllerContext.HttpContext);

            //var item = Activator.CreateInstance(commandType);
            //bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => item, commandType);

            //return item;
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var o = base.BindModel(controllerContext, bindingContext);

            return o;

            //var model = base.BindModel(controllerContext, bindingContext);

            //if (model != null)
            //{
            //    this.teclyn.Get<IIocContainer>().Inject(model);
            //}

            //return model;
        }
    }
}