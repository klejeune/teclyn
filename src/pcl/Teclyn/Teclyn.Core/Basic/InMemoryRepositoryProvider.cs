using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Teclyn.Core.Commands;
using Teclyn.Core.Domains;
using Teclyn.Core.Storage;

namespace Teclyn.Core.Basic
{
    public class InMemoryRepositoryProvider<T> : IRepositoryProvider<T> where T : class, IAggregate
    {
        private readonly IDictionary<string, T> data = new Dictionary<string, T>();

        public Type ElementType => typeof(T);

        public Expression Expression => this.data.Values.AsQueryable().Expression;

        public IQueryProvider Provider => data.Values.AsQueryable().Provider;

        public Task Create(T item)
        {
            this.data.Add(item.Id, item);

            return Task.FromResult(Type.Missing);
        }

        public Task Delete(T item)
        {
            this.data.Remove(item.Id);

            return Task.FromResult(Type.Missing);
        }

        public Task<bool> Exists(string id)
        {
            return Task.FromResult(this.data.ContainsKey(id));
        }

        public Task<T> GetByIdOrNull(Id<T> id)
        {
            return Task.FromResult(this.data.GetValueOrDefault(id.Value));
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.data.Values.GetEnumerator();
        }

        public Task Save(T item)
        {
            this.data[item.Id] = item;

            return Task.FromResult(Type.Missing);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}