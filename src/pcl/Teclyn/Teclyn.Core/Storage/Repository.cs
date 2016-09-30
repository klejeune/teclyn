using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Teclyn.Core.Domains;
using Teclyn.Core.Ioc;

namespace Teclyn.Core.Storage
{
    public class Repository<TAggregate> : IRepository<TAggregate> where TAggregate : class, IAggregate
    {
        [Inject]
        public IStorageConfiguration StorageConfiguration { get; set; }

        [Inject]
        public RepositoryService RepositoryService { get; set; }

        private IRepositoryProvider<TAggregate> provider;

        private IRepositoryProvider<TAggregate> GetProvider()
        {
            if (provider == null)
            {
                var info = this.RepositoryService.GetInfo<TAggregate>();
                this.provider = this.StorageConfiguration.GetRepositoryProvider<TAggregate>(info.CollectionName);
            }

            return this.provider;
        }

        public async Task<bool> Exists(string id)
        {
            return await this.GetProvider().Exists(id);
        }

        public async Task Create(TAggregate item)
        {
            await this.GetProvider().Create(item);
        }

        public async Task Delete(TAggregate item)
        {
            await this.GetProvider().Delete(item);
        }

        public async Task<TAggregate> GetById(string id)
        {
            var aggregate = await this.GetProvider().GetByIdOrNull(id);

            if (aggregate == null)
            {
                throw new NoSuchAggregateException();
            }

            return aggregate;
        }

        public async Task<TAggregate> GetByIdOrNull(string id)
        {
            if (id == null)
            {
                return null;
            }

            return await this.GetProvider().GetByIdOrNull(id);
        }

        public async Task Save(TAggregate item)
        {
            await this.GetProvider().Save(item);
        }

        public IEnumerator GetEnumerator()
        {
            return this.GetProvider().GetEnumerator();
        }

        IEnumerator<TAggregate> IEnumerable<TAggregate>.GetEnumerator()
        {
            return this.GetProvider().GetEnumerator();
        }

        public Type ElementType { get { return typeof(TAggregate); } }
        public Expression Expression { get { return this.GetProvider().Expression; } }
        public IQueryProvider Provider { get { return this.GetProvider().Provider; } }
    }
}