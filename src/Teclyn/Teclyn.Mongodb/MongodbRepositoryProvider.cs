using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;
using Teclyn.Core.Commands;
using Teclyn.Core.Domains;
using Teclyn.Core.Storage;
using Teclyn.Core.Tools;

namespace Teclyn.Mongodb
{
    public class MongodbRepositoryProvider<TInterface, TImplementation> : IRepositoryProvider<TInterface> 
        where TInterface : class, IAggregate
        where TImplementation : TInterface
    {
        private IMongoCollection<TImplementation> collection;

        public MongodbRepositoryProvider(IMongoDatabase database, string collectionName)
        {
            this.collection = database.GetCollection<TImplementation>(collectionName);
        }

        public IEnumerator<TInterface> GetEnumerator()
        {
            return this.collection.AsQueryable().GetEnumerator().Cast<TInterface, TImplementation>();
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Type ElementType => typeof(TInterface);
        public Expression Expression => this.collection.AsQueryable().Expression;
        public IQueryProvider Provider => this.collection.AsQueryable().Provider;
        public Task<TInterface> GetByIdOrNull(Id<TInterface> id)
        {
            var stringId = id.Value;
            return Task.FromResult(this.collection.AsQueryable().FirstOrDefault(i => i.Id == stringId).SafeCast<TInterface>());
        }

        public Task Create(TInterface item)
        {
            this.collection.InsertOne((TImplementation) item);

            return Task.FromResult(Type.Missing);
        }

        public Task Save(TInterface item)
        {
            var typedItem = (TImplementation) item;
            this.collection.ReplaceOne(i => i.Id == item.Id, typedItem);

            return Task.FromResult(Type.Missing);
        }

        public Task Delete(TInterface item)
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