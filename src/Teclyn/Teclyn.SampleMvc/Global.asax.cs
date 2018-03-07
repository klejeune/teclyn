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
using Teclyn.Core.Api;
using Teclyn.Core.Events;
using Teclyn.SampleCore.TodoLists.Events;
using Teclyn.SampleCore.Todos.Events;
using Teclyn.SampleMvc.App_Start;

namespace Teclyn.SampleMvc
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected async void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var configuration = new TeclynWebConfiguration();
            //configuration.UseStructureMap(StructuremapMvc.Container);
            //configuration.UseMongodbDatabase("TeclynSampleMVC");
            //var teclyn = new TeclynApi(configuration);
            //var eventService = teclyn.Get<IEventService>();

            //configuration.DropDatabase();

            //await eventService.Raise(new TodoListCreatedEvent
            //{
            //    AggregateId = "list-1",
            //    Name = "First List",
            //});

            //await eventService.Raise(new TodoListCreatedEvent
            //{
            //    AggregateId = "list-2",
            //    Name = "Second List",
            //});

            //await eventService.Raise(new TodoListCreatedEvent
            //{
            //    AggregateId = "list-3",
            //    Name = "Third List",
            //});

            //await eventService.Raise(new TodoListCreatedEvent
            //{
            //    AggregateId = "list-4",
            //    Name = "Fourth List",
            //});

            //await eventService.Raise(new TodoCreatedEvent
            //{
            //    AggregateId = "todo-10",
            //    TodoListName = "First List",
            //    TodoListId = "list-1",
            //    Text = "Test"
            //});
        }
    }
}
