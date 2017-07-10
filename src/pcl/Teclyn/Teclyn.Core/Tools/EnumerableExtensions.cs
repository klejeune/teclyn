using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

public static class EnumerableExtensions
{
    public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default(TValue))
    {
        TValue value;

        if (!dictionary.TryGetValue(key, out value))
        {
            value = defaultValue;
        }

        return value;
    }

    public static T[] AsArray<T>(this T item)
    {
        return new T[] { item };
    }

    public static T SafeCast<T>(this T item)
    {
        return item;
    }

    /// <summary>
    /// For more information on implementation to interface casting:
    /// https://stackoverflow.com/questions/40954155/how-to-cast-a-generic-t-in-repositoryt-to-an-interface-to-access-an-interface
    /// </summary>
    public static IEnumerator<TInterface> Cast<TInterface, TClass>(this IEnumerator<TClass> baseEnumerator)
        where TClass : TInterface
    {
        return new ImplementationEnumerator<TInterface, TClass>(baseEnumerator);
    }

    private class ImplementationEnumerator<TInterface, TClass> : IEnumerator<TInterface>
        where TClass : TInterface
    {
        private IEnumerator<TClass> baseEnumerator;

        public ImplementationEnumerator(IEnumerator<TClass> baseEnumerator)
        {
            this.baseEnumerator = baseEnumerator;
        }

        public void Dispose()
        {
            this.baseEnumerator.Dispose();
        }

        public bool MoveNext()
        {
            return this.baseEnumerator.MoveNext();
        }

        public void Reset()
        {
            this.baseEnumerator.Reset();
        }

        public TInterface Current => this.baseEnumerator.Current;

        object IEnumerator.Current => this.Current;
    }


}