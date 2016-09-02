using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Teclyn.Core.Basic;
using Teclyn.Core.Commands;
using Teclyn.Core.Domains;
using Teclyn.Core.Errors;
using Teclyn.Core.Events;
using Teclyn.Core.Events.Handlers;
using Teclyn.Core.Ioc;
using Teclyn.Core.Jobs;
using Teclyn.Core.Security.Context;
using Teclyn.Core.Storage;
using Teclyn.Core.Storage.EventHandlers;

namespace Teclyn.Core
{
    public class TeclynApi
    {
        public IEnumerable<ITeclynPlugin> Plugins { get; private set; }

        private IIocContainer iocContainer;
        private RepositoryService repositories { get; set; }

        public bool Debug { get; private set; }

        private TeclynApi()
        {
        }

        private void Fill(ITeclynConfiguration configuration)
        {
            this.Debug = configuration.Debug;
            this.RegisterSpecialServices(configuration);
        }

        private void RegisterSpecialServices(ITeclynConfiguration configuration)
        {
            // service registration
            this.iocContainer.Register(this.iocContainer);
            this.iocContainer.RegisterSingleton<RepositoryService>();
            this.iocContainer.Register<ITeclynContext, TeclynContext>();
            this.iocContainer.Register<IStorageConfiguration>(configuration.StorageConfiguration ?? new BasicStorageConfiguration());
            this.iocContainer.RegisterSingleton<EventHandlerService>();
            this.iocContainer.RegisterSingleton<CommandRepository>();

            // configuration analysis
            this.repositories = this.iocContainer.Get<RepositoryService>();
            this.repositories.Register(typeof(IEventInformation), typeof(EventInformation<>), "Event");
            this.ComputeAttributes(this.Plugins.Select(plugin => plugin.GetType().GetTypeInfo().Assembly));
            
            // computing based on the analyzed data
            
        }

        public static TeclynApi Initialize(ITeclynConfiguration configuration)
        {
            TeclynApi teclyn = new TeclynApi();

            teclyn.Plugins = configuration.Plugins.ToList();

            teclyn.iocContainer = GetContainer(configuration);
            teclyn.iocContainer.Initialize(teclyn.Plugins.Select(plugin => plugin.GetType().GetTypeInfo().Assembly));
            teclyn.iocContainer.Register(configuration.IocContainer);
            teclyn.iocContainer.Register(teclyn);

            teclyn.Fill(configuration);

            foreach (var plugin in teclyn.Plugins)
            {
                plugin.Initialize(teclyn);
            }

            var threadManager = teclyn.iocContainer.Get<IBackgroundThreadManager>();

            threadManager.Start();

            return teclyn;
        }

        private static IIocContainer GetContainer(ITeclynConfiguration configuration)
        {
            var ioc = configuration.IocContainer;

            if (ioc == null)
            {
                ioc = new BasicIocContainer();
            }

            return ioc;
        }

        public T Get<T>()
        {
            return this.iocContainer.Get<T>();
        }

        public object Get(Type type)
        {
            return this.iocContainer.Get(type);
        }

        private void ComputeAttributes(IEnumerable<Assembly> assemblies)
        {
            var attributeComputer = new AttributeComputer();
            attributeComputer.RegisterHandler(
                new[] {typeof(AggregateAttribute), typeof(AggregateImplementationAttribute)},
                dictionary =>
                {
                    foreach (var aggregateTypeInfo in dictionary[typeof(AggregateAttribute)])
                    {
                        var implementationTypeInfo =
                            dictionary[typeof(AggregateImplementationAttribute)].SingleOrDefault(
                                implementation =>
                                    aggregateTypeInfo.Type.GetTypeInfo()
                                        .IsAssignableFrom(implementation.Type.GetTypeInfo()));

                        if (implementationTypeInfo != null)
                        {
                            this.repositories.Register(aggregateTypeInfo.Type, implementationTypeInfo.Type, implementationTypeInfo.Type.Name);
                        }
                        else if (aggregateTypeInfo.Type.GetTypeInfo().IsClass)
                        {
                            this.repositories.Register(aggregateTypeInfo.Type, aggregateTypeInfo.Type, aggregateTypeInfo.Type.Name);
                        }
                        else
                        {
                            throw new InvalidOperationException($"The Aggregate {aggregateTypeInfo.Type.Name} doesn't have any implementation.");
                        }
                    }
                });

            attributeComputer.RegisterHandler(new[] {typeof(EventHandlerAttribute)},
                dictionary =>
                {
                    var eventHandlerTypes = dictionary[typeof(EventHandlerAttribute)];
                    var eventHandlerService = this.Get<EventHandlerService>();

                    foreach (var eventHandlerType in eventHandlerTypes)
                    {
                        eventHandlerService.RegisterEventHandler(eventHandlerType.Type);
                    }
                });

            attributeComputer.RegisterHandler(new[] {typeof(RemoteAttribute)},
                dictionary =>
                {
                    var commandTypes = dictionary[typeof(RemoteAttribute)];
                    var commandService = this.Get<CommandService>();

                    foreach (var commandType in commandTypes)
                    {
                        commandService.RegisterCommand(commandType.Type);
                    }
                });

            attributeComputer.RegisterHandler(
                new[] { typeof(ServiceAttribute), typeof(ServiceImplementationAttribute) },
                dictionary =>
                {
                    foreach (var serviceTypeInfo in dictionary[typeof(ServiceAttribute)])
                    {
                        var implementationTypeInfo =
                            dictionary[typeof(ServiceImplementationAttribute)].SingleOrDefault(
                                implementation =>
                                    serviceTypeInfo.Type.GetTypeInfo()
                                        .IsAssignableFrom(implementation.Type.GetTypeInfo()));

                        if (implementationTypeInfo != null)
                        {
                            this.iocContainer.Register(serviceTypeInfo.Type, implementationTypeInfo.Type);
                        }
                        else if (serviceTypeInfo.Type.GetTypeInfo().IsClass)
                        {
                            this.iocContainer.Register(serviceTypeInfo.Type, serviceTypeInfo.Type);
                        }
                        else
                        {
                            throw new InvalidOperationException($"The Service {serviceTypeInfo.Type.Name} doesn't have any implementation.");
                        }
                    }
                });

            attributeComputer.Compute(assemblies);
        }

        public void RegisterRepository<TAggregate>(string collectionName = null)
        {
            if (string.IsNullOrWhiteSpace(collectionName))
            {
                collectionName = typeof(TAggregate).Name;
            }

            this.repositories.Register(typeof(TAggregate), typeof(TAggregate), collectionName);
        }
    }
}