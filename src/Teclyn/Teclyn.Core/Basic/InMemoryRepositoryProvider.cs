using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Teclyn.Core.Commands;
using Teclyn.Core.Domains;
using Teclyn.Core.Storage;
using Teclyn.Core.Tools;

namespace Teclyn.Core.Basic
{
    public class InMemoryRepositoryProvider<T> : IRepositoryProvider<T> where T : IAggregate
    {
        private static readonly IDictionary<string, T> Data = new Dictionary<string, T>();

        public Type ElementType => typeof(T);

        public Expression Expression => Data.Values.AsQueryable().Expression;

        public IQueryProvider Provider => Data.Values.AsQueryable().Provider;

        public Task Create(T item)
        {
            Data.Add(item.Id, item);

            return Task.FromResult(Type.Missing);
        }

        public Task Delete(T item)
        {
            Data.Remove(item.Id);

            return Task.FromResult(Type.Missing);
        }

        public Task<bool> Exists(string id)
        {
            return Task.FromResult(Data.ContainsKey(id));
        }

        public Task<T> GetByIdOrNull(Id<T> id)
        {
            return Task.FromResult(Data.GetValueOrDefault(id.Value));
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Data.Values.GetEnumerator();
        }

        public Task Save(T item)
        {
            Data[item.Id] = item;

            return Task.FromResult(Type.Missing);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}