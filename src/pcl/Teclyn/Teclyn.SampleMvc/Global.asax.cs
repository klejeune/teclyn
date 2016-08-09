using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using StructureMap;
using StructureMap.Graph;
using StructureMap.Graph.Scanning;
using Teclyn.Core;
using Teclyn.Core.Events;
using Teclyn.SampleCore.TodoLists.Events;
using Teclyn.SampleMvc.App_Start;

namespace Teclyn.SampleMvc
{
    public class MvcApplication : System.Web.HttpApplication
    {
        //public static TeclynApi Teclyn { get; private set; }
       // public static IContainer StructureMapContainer { get; private set; }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var configuration = new TeclynWebConfiguration(StructuremapMvc.Container);
            //configuration.SetMongodbDatabase("TeclynSampleMVC");
            var teclyn = TeclynApi.Initialize(configuration);
            var eventService = teclyn.Get<EventService>();

            configuration.DropDatabase();

            eventService.Raise(new TodoListCreatedEvent
            {
                AggregateId = "list-1",
                Name = "First List",
            });

            eventService.Raise(new TodoListCreatedEvent
            {
                AggregateId = "list-2",
                Name = "Second List",
            });

            eventService.Raise(new TodoListCreatedEvent
            {
                AggregateId = "list-3",
                Name = "Third List",
            });

            eventService.Raise(new TodoListCreatedEvent
            {
                AggregateId = "list-4",
                Name = "Fourth List",
            });
        }
    }
}
