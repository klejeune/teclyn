using System.Web.Hosting;
using System.Web.Mvc;
using Teclyn.AspNetMvc.Commands;
using Teclyn.AspNetMvc.Commands.Renderers;
using Teclyn.AspNetMvc.Integration;
using Teclyn.AspNetMvc.ModelBinders;
using Teclyn.AspNetMvc.VirtualFileSystem;
using Teclyn.Core;
using Teclyn.Core.Commands;
using Teclyn.Core.Ioc;
using IDependencyResolver = Teclyn.Core.Ioc.IDependencyResolver;

namespace Teclyn.AspNetMvc
{
    public class TeclynAspNetMvcPlugin
    {
        public string Name => "Teclyn.AspNetMvc";
        public void Initialize(IDependencyResolver teclyn)
        {
            //MvcApplicationExtensions.SetTeclyn(teclyn);

            // Singletons
            //teclyn.Get<IDependencyResolver>().RegisterSingleton<CommandPropertyRendererFactory>();

            // Renderers
            teclyn.Get<CommandPropertyRendererFactory>().RegisterRenderer(new IdRenderer());
            teclyn.Get<CommandPropertyRendererFactory>().RegisterRenderer(new StringRenderer());
            teclyn.Get<CommandPropertyRendererFactory>().RegisterRenderer(new BooleanRenderer());
            teclyn.Get<CommandPropertyRendererFactory>().RegisterRenderer(new DateTimeRenderer());
            teclyn.Get<CommandPropertyRendererFactory>().RegisterRenderer(new DoubleRenderer());
            teclyn.Get<CommandPropertyRendererFactory>().RegisterRenderer(new IntegerRenderer());

            // MVC
           // System.Web.Mvc.ModelBinders.Binders.Add(typeof(IBaseCommand), new CommandModelBinder(teclyn));
            System.Web.Mvc.ModelBinders.Binders.Add(typeof(string), new IdModelBinder());
            HostingEnvironment.RegisterVirtualPathProvider(new TeclynVirtualPathProvider());
            //DependencyResolver.SetResolver(teclyn.Get<TeclynBasicServiceLocator>());
        }
    }
}
