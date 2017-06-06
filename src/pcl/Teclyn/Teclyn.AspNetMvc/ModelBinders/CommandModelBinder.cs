using System;
using System.ComponentModel;
using System.Linq;
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
            
            var item = Activator.CreateInstance(commandType);
            bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => item, commandType);

            return item;
        }

        protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext,
            PropertyDescriptor propertyDescriptor)
        {
            //if (propertyDescriptor.PropertyType.IsGenericType &&
            //    propertyDescriptor.PropertyType.GetGenericTypeDefinition() == typeof(Id<>))
            //{
            //    var idType = propertyDescriptor.PropertyType.GetGenericArguments().Single();
            //    var value = controllerContext.HttpContext.Request.Form[propertyDescriptor.Name];

            //    var id = Id.From(idType, value);

            //    propertyDescriptor.SetValue(bindingContext.Model, id);
            //}
            //else
            {
                base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
            }
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var model = base.BindModel(controllerContext, bindingContext);

            if (model != null)
            {
                this.teclyn.Get<IIocContainer>().Inject(model);
            }

            return model;
        }
    }
}