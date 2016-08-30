using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        public T GetByIdOrNull(string id)
        {
            return this.collection.AsQueryable().FirstOrDefault(i => i.Id == id);
        }

        public void Create(T item)
        {
            this.collection.InsertOne(item);
        }

        public void Save(T item)
        {
            this.collection.ReplaceOne(i => i.Id == item.Id, item);
        }

        public void Delete(T item)
        {
            this.collection.DeleteOne(i => i.Id == item.Id);
        }

        public bool Exists(string id)
        {
            return this.collection.AsQueryable().Any(i => i.Id == id);
        }
    }
}