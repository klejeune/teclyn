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
    public class Rep
    {
        public static IRepositoryProvider<TInterface> GetProviderGenerics<TInterface, TImplementation>(IStorageConfiguration storageConfiguration, string collectionName)
            where TInterface : class, IAggregate
            where TImplementation : TInterface
        {
            return storageConfiguration.GetRepositoryProvider<TInterface, TImplementation>(collectionName);
        }
    }

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

                var method =
                    ReflectionTools.Static.Method(
                        () => Rep.GetProviderGenerics<IDummyAggregate, DummyAggregate>(null, null))
                        .GetGenericMethodDefinition();

                this.provider = (IRepositoryProvider<TAggregate>)
                    method.MakeGenericMethod(info.AggregateType, info.ImplementationType)
                    .Invoke(null, new object[] { this.StorageConfiguration, info.CollectionName });
            }

            return this.provider;
        }

        private static IRepositoryProvider<TInterface> GetProviderGenerics<TInterface, TImplementation>(IStorageConfiguration storageConfiguration, string collectionName)
            where TInterface : class, IAggregate
            where TImplementation : TInterface
        {
            return storageConfiguration.GetRepositoryProvider<TInterface, TImplementation>(collectionName);
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

        public async Task<TAggregate> GetById(Id<TAggregate> id)
        {
            var aggregate = await this.GetProvider().GetByIdOrNull(id);

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