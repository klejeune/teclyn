using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Teclyn.Core.Ioc
{
    public interface IDependencyResolver
    {
        T Get<T>();
        object Get(Type type);
    }
}