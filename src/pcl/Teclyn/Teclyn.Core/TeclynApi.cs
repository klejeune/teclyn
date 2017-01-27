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
using Teclyn.Core.Metadata;
using Teclyn.Core.Security.Context;
using Teclyn.Core.Storage;
using Teclyn.Core.Storage.EventHandlers;

namespace Teclyn.Core
{
    public class TeclynApi
    {
        public IEnumerable<ITeclynPlugin> Plugins { get; }
        public IEnumerable<Assembly> ScannedAssemblies { get; }

        private readonly IIocContainer iocContainer;
        private RepositoryService repositories;

        public bool Debug { get; private set; }

        public TeclynApi() : this(new TeclynDefaultConfiguration())
        {
        }

        public TeclynApi(ITeclynConfiguration configuration)
        {
            this.Plugins = configuration.Plugins.ToList();
            this.ScannedAssemblies = this.Plugins.Select(plugin => plugin.GetType().GetTypeInfo().Assembly).ToList();

            this.iocContainer = GetContainer(configuration);
            this.iocContainer.Initialize(this.ScannedAssemblies);
            this.iocContainer.Register(this.iocContainer);
            this.iocContainer.Register(this);

            this.Fill(configuration);

            foreach (var plugin in this.Plugins)
            {
                plugin.Initialize(this);
            }

            var threadManager = this.iocContainer.Get<IBackgroundThreadManager>();

            threadManager.Start();
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
            this.iocContainer.RegisterSingleton<MetadataRepository>();

            // configuration analysis
            this.repositories = this.iocContainer.Get<RepositoryService>();
            this.repositories.Register(typeof(IEventInformation), typeof(EventInformation<>), "Event", null, null);
            this.ComputeAttributes(this.ScannedAssemblies);

            // computing based on the analyzed data

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
                new Predicate<Type>[]
                {
                    type => type.GetTypeInfo().GetCustomAttributes<AggregateAttribute>().Any(),
                    type => type.GetTypeInfo().GetCustomAttributes<AggregateImplementationAttribute>().Any()
                },
                dictionary =>
                {
                    var aggregateAttributeTypes = dictionary.ElementAt(0).Value;
                    var aggregateImplementationAttributeTypes = dictionary.ElementAt(1).Value;

                    foreach (var aggregateTypeInfo in aggregateAttributeTypes)
                    {
                        var implementationTypeInfo =
                            aggregateImplementationAttributeTypes.SingleOrDefault(
                                implementation =>
                                    aggregateTypeInfo.GetTypeInfo()
                                        .IsAssignableFrom(implementation.GetTypeInfo()));

                        var attribute = aggregateTypeInfo.GetTypeInfo().GetCustomAttribute<AggregateAttribute>();

                        if (implementationTypeInfo != null)
                        {
                            this.repositories.Register(
                                aggregateTypeInfo,
                                implementationTypeInfo,
                                implementationTypeInfo.Name,
                                attribute.AccessController,
                                attribute.DefaultFilter);
                        }
                        else if (aggregateTypeInfo.GetTypeInfo().IsClass)
                        {
                            this.repositories.Register(
                                aggregateTypeInfo,
                                aggregateTypeInfo,
                                aggregateTypeInfo.Name,
                                attribute.AccessController,
                                attribute.DefaultFilter);
                        }
                        else
                        {
                            throw new InvalidOperationException($"The Aggregate {aggregateTypeInfo.Name} doesn't have any implementation.");
                        }
                    }
                });

            attributeComputer.RegisterHandler(new Predicate<Type>[]
            {
                type => type.GetTypeInfo().GetCustomAttribute<EventHandlerAttribute>() != null
            },
                dictionary =>
                {
                    var eventHandlerTypes = dictionary.ElementAt(0).Value;
                    var eventHandlerService = this.Get<EventHandlerService>();

                    foreach (var eventHandlerType in eventHandlerTypes)
                    {
                        eventHandlerService.RegisterEventHandler(eventHandlerType);
                    }
                });

            attributeComputer.RegisterHandler(new Predicate<Type>[]
            {
                type => typeof(ICommand).GetTypeInfo().IsAssignableFrom(type.GetTypeInfo()) && !type.GetTypeInfo().IsAbstract && !type.GetTypeInfo().IsInterface
            },
                dictionary =>
                {
                    var commandTypes = dictionary.ElementAt(0).Value;
                    var commandService = this.Get<CommandService>();

                    foreach (var commandType in commandTypes)
                    {
                        commandService.RegisterCommand(commandType);
                    }
                });

            attributeComputer.RegisterHandler(new Predicate<Type>[]
            {
                type => typeof(ITeclynEvent).GetTypeInfo().IsAssignableFrom(type.GetTypeInfo()) && !type.GetTypeInfo().IsAbstract && !type.GetTypeInfo().IsInterface
            },
                dictionary =>
                {
                    var commandTypes = dictionary.ElementAt(0).Value;
                    var eventService = this.Get<EventService>();

                    foreach (var commandType in commandTypes)
                    {
                        eventService.RegisterEvent(commandType);
                    }
                });

            attributeComputer.RegisterHandler(
                new Predicate<Type>[]
                {
                     type => type.GetTypeInfo().GetCustomAttribute<ServiceAttribute>() != null,
                      type => type.GetTypeInfo().GetCustomAttribute<ServiceImplementationAttribute>() != null,
                },
                dictionary =>
                {
                    var serviceAttributeTypes = dictionary.ElementAt(0).Value;
                    var serviceImplementationAttributeTypes = dictionary.ElementAt(1).Value;

                    foreach (var serviceTypeInfo in serviceAttributeTypes)
                    {
                        var implementationTypeInfo =
                            serviceImplementationAttributeTypes.SingleOrDefault(
                                implementation =>
                                    serviceTypeInfo.GetTypeInfo()
                                        .IsAssignableFrom(implementation.GetTypeInfo()));

                        if (implementationTypeInfo != null)
                        {
                            this.iocContainer.Register(serviceTypeInfo, implementationTypeInfo);
                        }
                        else if (serviceTypeInfo.GetTypeInfo().IsClass)
                        {
                            this.iocContainer.Register(serviceTypeInfo, serviceTypeInfo);
                        }
                        else
                        {
                            throw new InvalidOperationException($"The Service {serviceTypeInfo.Name} doesn't have any implementation.");
                        }
                    }
                });

            attributeComputer.Compute(assemblies);
        }
    }
}