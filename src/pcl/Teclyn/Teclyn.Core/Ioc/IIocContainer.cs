using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Teclyn.Core.Ioc
{
    public interface IIocContainer
    {
        void Initialize(IEnumerable<Assembly> assemblies);
        T Get<T>();
        object Get(Type type);
        void Register<TPublicType, TImplementation>() where TImplementation : TPublicType;
        void Register<TPublicType>() where TPublicType : class;
        void RegisterSingleton<TPublicType>() where TPublicType : class;
        void RegisterSingleton<TPublicType, TImplementation>()
            where TPublicType : class
            where TImplementation : class, TPublicType;
        void Register<TPublicType>(TPublicType @object);
        void Register(Type publicType, Type implementationType);
        void Inject(object item);
    }
}