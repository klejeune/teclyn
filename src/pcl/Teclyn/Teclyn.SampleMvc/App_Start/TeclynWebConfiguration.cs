using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using MongoDB.Driver;
using StructureMap;
using Teclyn.AspNetMvc;
using Teclyn.Core;
using Teclyn.Core.Basic;
using Teclyn.Core.Domains;
using Teclyn.Core.Ioc;
using Teclyn.Core.Security.Context;
using Teclyn.Core.Storage;
using Teclyn.Mongodb;
using Teclyn.SampleCore;
using Teclyn.StructureMap;

namespace Teclyn.SampleMvc
{
    public class TeclynWebConfiguration : ITeclynConfiguration, IStorageConfiguration
    {
        public IIocContainer IocContainer { get; private set; }
        public IStorageConfiguration StorageConfiguration => this;

        private IMongoDatabase mongoDatabase;
        private string databaseName;

        public IEnumerable<ITeclynPlugin> Plugins => new ITeclynPlugin[]
        {
            new TeclynCorePlugin(),
            new TeclynStructureMapPlugin(),
            new TeclynAspNetMvcPlugin(),
            new SampleCorePlugin(),
            new SampleWebPlugin(), 
        };

        public bool Debug => true;

        public IRepositoryProvider<TInterface> GetRepositoryProvider<TInterface, TImplementation>(string collectionName) 
            where TInterface : class, IAggregate
            where TImplementation : TInterface
        {
            if (this.mongoDatabase != null)
            {
                return new MongodbRepositoryProvider<TInterface, TImplementation>(this.mongoDatabase, collectionName);
            }
            else
            {
                return new InMemoryRepositoryProvider<TInterface>();
            }
        }

        public TeclynWebConfiguration UseMongodbDatabase(string databaseName)
        {
            this.databaseName = databaseName;
            var mongoClient = new MongoClient(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            this.mongoDatabase = mongoClient.GetDatabase(databaseName);

            return this;
        }

        public TeclynWebConfiguration UseStructureMap(IContainer structureMapContainer)
        {
            this.IocContainer = new StructureMapContainer(structureMapContainer);

            return this;
        }

        public void DropDatabase()
        {
            if (this.mongoDatabase != null)
            {
                this.mongoDatabase.Client.DropDatabase(databaseName);
            }
        }
    }
}