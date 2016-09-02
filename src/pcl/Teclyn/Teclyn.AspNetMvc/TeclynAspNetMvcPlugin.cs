using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Teclyn.AspNetMvc.Commands;
using Teclyn.AspNetMvc.Commands.Renderers;
using Teclyn.AspNetMvc.Integration;
using Teclyn.AspNetMvc.ModelBinders;
using Teclyn.AspNetMvc.Mvc.Controllers;
using Teclyn.AspNetMvc.VirtualFileSystem;
using Teclyn.Core;
using Teclyn.Core.Commands;
using Teclyn.Core.Ioc;

namespace Teclyn.AspNetMvc
{
    public class TeclynAspNetMvcPlugin : ITeclynPlugin
    {
        public string Name => "Teclyn.AspNetMvc";
        public void Initialize(TeclynApi teclyn)
        {
            MvcApplicationExtensions.SetTeclyn(teclyn);

            // Singletons
            teclyn.Get<IIocContainer>().RegisterSingleton<CommandPropertyRendererFactory>();

            // Renderers
            teclyn.Get<CommandPropertyRendererFactory>().RegisterRenderer(new StringRenderer());
            teclyn.Get<CommandPropertyRendererFactory>().RegisterRenderer(new BooleanRenderer());
            teclyn.Get<CommandPropertyRendererFactory>().RegisterRenderer(new DateTimeRenderer());
            teclyn.Get<CommandPropertyRendererFactory>().RegisterRenderer(new DoubleRenderer());
            teclyn.Get<CommandPropertyRendererFactory>().RegisterRenderer(new IntegerRenderer());

            // MVC
            System.Web.Mvc.ModelBinders.Binders.Add(typeof(IBaseCommand), new CommandModelBinder(teclyn));
            HostingEnvironment.RegisterVirtualPathProvider(new TeclynVirtualPathProvider());
            DependencyResolver.SetResolver(teclyn.Get<TeclynBasicServiceLocator>());
        }
    }
}
