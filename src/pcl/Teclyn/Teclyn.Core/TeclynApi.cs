using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Teclyn.Core.Configuration;
using Teclyn.Core.Domains;
using Teclyn.Core.Events;
using Teclyn.Core.Events.Handlers;
using Teclyn.Core.Ioc;
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

        private TeclynApi()
        {
        }

        private void Fill(ITeclynConfiguration configuration)
        {
            this.RegisterSpecialServices(configuration);
        }

        private void RegisterSpecialServices(ITeclynConfiguration configuration)
        {
            // service registration
            this.iocContainer.Register(this.iocContainer);
            this.iocContainer.RegisterSingleton<RepositoryService>();
            this.iocContainer.Register<IEnvironment>(configuration.Environment);
            this.iocContainer.Register<ITeclynContext, TeclynContext>();
            this.iocContainer.Register<IStorageConfiguration>(configuration.StorageConfiguration);
            this.iocContainer.RegisterSingleton<EventHandlerService>();
            configuration.RegisterServices();

            // configuration analysis
            this.repositories = this.iocContainer.Get<RepositoryService>();
            this.ComputeAttributes(configuration);
            
            // computing based on the analyzed data
            
        }

        public static TeclynApi Initialize(ITeclynConfiguration configuration)
        {
            TeclynApi teclyn = new TeclynApi();

            teclyn.Plugins = configuration.Plugins.ToList();

            teclyn.iocContainer = GetContainer(configuration);
            teclyn.iocContainer.Initialize(configuration.Plugins.Select(plugin => plugin.GetType().GetTypeInfo().Assembly));
            teclyn.iocContainer.Register(configuration.IocContainer);
            teclyn.iocContainer.Register(teclyn);

            teclyn.Fill(configuration);

            foreach (var plugin in teclyn.Plugins)
            {
                plugin.Initialize(teclyn);
            }

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

        private void ComputeAttributes(ITeclynConfiguration configuration)
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

            attributeComputer.Compute(configuration);
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