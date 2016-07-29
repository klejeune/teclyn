using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class TypeTools
{
    public static IEnumerable<Type> GetAllAncestors(this Type type)
    {
        var typeInfo = type.GetTypeInfo();

        if (typeInfo.BaseType == typeof(object))
        {
            return type.AsArray();
        }
        else
        {
            return type.AsArray().Union(GetAllAncestors(typeInfo.BaseType));
        }
    }

    public static IEnumerable<Type> GetAllAncestorsAndInterfaces(this Type type)
    {
        return type.GetTypeInfo().ImplementedInterfaces.Union(GetAllAncestors(type)).ToList();
    }
}