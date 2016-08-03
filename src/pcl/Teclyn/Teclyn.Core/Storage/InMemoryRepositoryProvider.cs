using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Teclyn.Core.Domains;

namespace Teclyn.Core.Storage
{
    public class InMemoryRepositoryProvider<T> : IRepositoryProvider<T> where T : class, IAggregate
    {
        private IDictionary<string, T> data = new Dictionary<string, T>();

        public Type ElementType
        {
            get
            {
                return typeof(T);
            }
        }

        public Expression Expression
        {
            get { return this.data.Values.AsQueryable().Expression; }
        }

        public IQueryProvider Provider
        {
            get { return data.Values.AsQueryable().Provider; }
        }

        public void Create(T item)
        {
            this.data.Add(item.Id, item);
        }

        public void Delete(string id)
        {
            this.data.Remove(id);
        }

        public T GetByIdOrNull(string id)
        {
            return this.data.GetValueOrDefault(id);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.data.Values.GetEnumerator();
        }

        public void Save(T item)
        {
            this.data[item.Id] = item;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}