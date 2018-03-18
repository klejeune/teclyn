﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using StructureMap;
using Teclyn.Core.Events.Handlers;
using Teclyn.Core.Ioc;

namespace Teclyn.StructureMap
{
    public class StructureMapContainer : IIocContainer
    {
        private IContainer container;

        public StructureMapContainer(IContainer container)
        {
            this.container = container;
        }

        public void Initialize(IEnumerable<Assembly> assemblies)
        {
            this.container.Configure(_ =>
            {
                _.Policies.SetAllProperties(convention => convention.Matching(property => property.GetCustomAttribute<InjectAttribute>() != null));

                _.Scan(x =>
                {
                    foreach (var assembly in assemblies)
                    {
                        x.Assembly(assembly);
                    }

                    x.WithDefaultConventions();


                });

                //_.ForSingletonOf<IEventHandlerService>();
            });
        }

        public T Get<T>()
        {
            return (T)this.Get(typeof(T));
        }

        public object Get(Type type)
        {
            try
            {
                return container.GetInstance(type);
            }
            catch (StructureMapConfigurationException)
            {
                return null;
            }
        }

        public void Register<TPublicType, TImplementation>() where TImplementation : TPublicType
        {
            container.Configure(_ => _.For<TPublicType>().Use<TImplementation>());
        }

        public void Register<TPublicType>() where TPublicType : class
        {
            container.Configure(_ => _.For<TPublicType>().Use<TPublicType>());
        }

        public void RegisterSingleton<TPublicType, TImplementation>() 
            where TPublicType : class
            where TImplementation : class, TPublicType
        {
            container.Configure(_ => _.For<TPublicType>().Use<TImplementation>().Singleton());
        }

        public void RegisterSingleton<TPublicType>() where TPublicType : class
        {
            container.Configure(_ => _.For<TPublicType>().Use<TPublicType>().Singleton());
        }

        public void Register<TPublicType>(TPublicType @object)
        {
            container.Configure(_ => _.For<TPublicType>().Use(() => @object).Singleton());
        }

        public void Register(Type publicType, Type implementationType)
        {
            container.Configure(_ => _.For(publicType).Use(implementationType).Singleton());
        }

        public void Inject(object item)
        {
            container.Inject(item);
        }
    }
}
