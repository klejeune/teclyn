using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Teclyn.Core.Commands;
using Teclyn.Core.Domains;
using Teclyn.Core.Dummies;
using Teclyn.Core.Ioc;
using Teclyn.Core.Tools;

namespace Teclyn.Core.Storage
{
    public class Repository<TAggregate> : IRepository<TAggregate> where TAggregate : IAggregate
    {
        private readonly IRepositoryProvider<TAggregate> _repositoryProvider;

        public Repository(IRepositoryProvider<TAggregate> repositoryProvider)
        {
            this._repositoryProvider = repositoryProvider;
        }
        
        public async Task<bool> Exists(string id)
        {
            return await this._repositoryProvider.Exists(id);
        }

        public async Task Create(TAggregate item)
        {
            await this._repositoryProvider.Create(item);
        }

        public async Task Delete(TAggregate item)
        {
            await this._repositoryProvider.Delete(item);
        }

        public async Task<TAggregate> GetById(Id<TAggregate> id)
        {

            if (id == null)
            {
                throw new TeclynException("The id parameter is null. Please provide an id.");
            }

            var aggregate = await this._repositoryProvider.GetByIdOrNull(id);

            if (aggregate == null)
            {
                throw new NoSuchAggregateException();
            }

            return aggregate;
        }

        public async Task<TAggregate> GetByIdOrNull(Id<TAggregate> id)
        {
            if (id == null)
            {
                return default(TAggregate);
            }

            return await this._repositoryProvider.GetByIdOrNull(id);
        }

        public async Task Save(TAggregate item)
        {
            await this._repositoryProvider.Save(item);
        }

        public IEnumerator GetEnumerator()
        {
            return this._repositoryProvider.GetEnumerator();
        }

        IEnumerator<TAggregate> IEnumerable<TAggregate>.GetEnumerator()
        {
            return this._repositoryProvider.GetEnumerator();
        }

        public Type ElementType => typeof(TAggregate);
        public Expression Expression => this._repositoryProvider.Expression;
        public IQueryProvider Provider => this._repositoryProvider.Provider;
    }
}