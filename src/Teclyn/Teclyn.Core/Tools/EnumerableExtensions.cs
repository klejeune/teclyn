using System.Collections;
using System.Collections.Generic;

namespace Teclyn.Core.Tools
{
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
            private readonly IEnumerator<TClass> _baseEnumerator;

            public ImplementationEnumerator(IEnumerator<TClass> baseEnumerator)
            {
                this._baseEnumerator = baseEnumerator;
            }

            public void Dispose()
            {
                this._baseEnumerator.Dispose();
            }

            public bool MoveNext()
            {
                return this._baseEnumerator.MoveNext();
            }

            public void Reset()
            {
                this._baseEnumerator.Reset();
            }

            public TInterface Current => this._baseEnumerator.Current;

            object IEnumerator.Current => this.Current;
        }


    }
}