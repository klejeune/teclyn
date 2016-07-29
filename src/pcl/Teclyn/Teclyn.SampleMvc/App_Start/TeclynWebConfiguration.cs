using System.Collections.Generic;
using StructureMap;
using Teclyn.AspNetMvc;
using Teclyn.Core;
using Teclyn.Core.Configuration;
using Teclyn.Core.Domains;
using Teclyn.Core.Ioc;
using Teclyn.Core.Security.Context;
using Teclyn.Core.Storage;
using Teclyn.SampleCore;
using Teclyn.StructureMap;

namespace Teclyn.SampleMvc
{
    public class TeclynWebConfiguration : ITeclynConfiguration, IStorageConfiguration, IEnvironment
    {
        public IIocContainer IocContainer { get; }
        public IEnvironment Environment => this;
        public IStorageConfiguration StorageConfiguration => this;
        public IEnumerable<ITeclynPlugin> Plugins => new ITeclynPlugin[]
        {
            new TeclynCorePlugin(),
            new TeclynStructureMapPlugin(),
            new TeclynAspNetMvcPlugin(),
            new SampleCorePlugin(),
        };
        public void RegisterServices()
        {
        }

        public IRepositoryProvider<T> GetRepositoryProvider<T>() where T : class, IAggregate
        {
            return new InMemoryRepositoryProvider<T>();
        }

        public TeclynWebConfiguration(IContainer structureMapContainer)
        {
            this.IocContainer = new StructureMapContainer(structureMapContainer);
        }

        public ITeclynUser GetCurrentUser()
        {
            throw new System.NotImplementedException();
        }
    }
}