using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public void Create(TAggregate item)
        {
            this.GetProvider().Create(item);
        }

        public void Delete(TAggregate item)
        {
            this.GetProvider().Delete(item);
        }

        public TAggregate GetById(string id)
        {
            var aggregate = this.GetProvider().GetByIdOrNull(id);

            if (aggregate == null)
            {
                throw new NoSuchAggregateException();
            }

            return aggregate;
        }

        public TAggregate GetByIdOrNull(string id)
        {
            if (id == null)
            {
                return null;
            }

            return this.GetProvider().GetByIdOrNull(id);
        }

        public void Save(TAggregate item)
        {
            this.GetProvider().Save(item);
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