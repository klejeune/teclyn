using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;
using Teclyn.Core.Domains;
using Teclyn.Core.Storage;

namespace Teclyn.Mongodb
{
    public class MongodbRepositoryProvider<T> : IRepositoryProvider<T> where T : class, IAggregate
    {
        private IMongoCollection<T> collection;

        public MongodbRepositoryProvider(IMongoDatabase database, string collectionName)
        {
            this.collection = database.GetCollection<T>(collectionName);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.collection.AsQueryable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Type ElementType => typeof(T);
        public Expression Expression => this.collection.AsQueryable().Expression;
        public IQueryProvider Provider => this.collection.AsQueryable().Provider;
        public Task<T> GetByIdOrNull(string id)
        {
            return Task.FromResult(this.collection.AsQueryable().FirstOrDefault(i => i.Id == id));
        }

        public Task Create(T item)
        {
            this.collection.InsertOne(item);

            return Task.FromResult(Type.Missing);
        }

        public Task Save(T item)
        {
            this.collection.ReplaceOne(i => i.Id == item.Id, item);

            return Task.FromResult(Type.Missing);
        }

        public Task Delete(T item)
        {
            this.collection.DeleteOne(i => i.Id == item.Id);

            return Task.FromResult(Type.Missing);
        }

        public Task<bool> Exists(string id)
        {
            return Task.FromResult(this.collection.AsQueryable().Any(i => i.Id == id));
        }
    }
}