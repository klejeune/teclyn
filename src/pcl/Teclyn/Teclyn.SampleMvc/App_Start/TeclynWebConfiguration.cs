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
        public IIocContainer IocContainer { get; }
        public IStorageConfiguration StorageConfiguration => this;

        private IMongoDatabase mongoDatabase;
        private string databaseName;

        public IEnumerable<ITeclynPlugin> Plugins => new ITeclynPlugin[]
        {
            new TeclynCorePlugin(),
            new TeclynStructureMapPlugin(),
            new TeclynAspNetMvcPlugin(),
            new SampleCorePlugin(),
        };

        public bool Debug => true;

        public IRepositoryProvider<T> GetRepositoryProvider<T>(string collectionName) where T : class, IAggregate
        {
            if (this.mongoDatabase != null)
            {
                return new MongodbRepositoryProvider<T>(this.mongoDatabase, collectionName);
            }
            else
            {
                return new InMemoryRepositoryProvider<T>();
            }
        }

        public TeclynWebConfiguration(IContainer structureMapContainer)
        {
            this.IocContainer = new StructureMapContainer(structureMapContainer);
        }

        public TeclynWebConfiguration SetMongodbDatabase(string databaseName)
        {
            this.databaseName = databaseName;
            var mongoClient = new MongoClient(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            this.mongoDatabase = mongoClient.GetDatabase(databaseName);

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