using System.Collections.Generic;
using StructureMap;
using Teclyn.Core.Configuration;
using Teclyn.Core.Domains;
using Teclyn.Core.Ioc;
using Teclyn.Core.Services;
using Teclyn.Core.Storage;
using Teclyn.StructureMap;

namespace Teclyn.Core.Tests
{
    public class TeclynTestConfiguration : ITeclynConfiguration, IStorageConfiguration
    {
        public StructureMapContainer TestIocContainer { get; }
        public TestEnvironment TestEnvironment { get; }

        public IIocContainer IocContainer { get { return this.TestIocContainer; } }
        public IEnvironment Environment { get { return this.TestEnvironment; } }
        public IStorageConfiguration StorageConfiguration { get { return this; } }

        public IEnumerable<ITeclynPlugin> Plugins => new ITeclynPlugin[]
        {
            new TeclynCorePlugin(), 
            new TeclynStructureMapPlugin(),
            new TeclynCoreTestsPlugin(),
        };

        public void RegisterServices()
        {
            this.TestIocContainer.Register<UserContext>();
        }

        public TeclynTestConfiguration()
        {
            this.TestIocContainer = new StructureMapContainer(new Container());
            this.TestEnvironment = new TestEnvironment();
        }

        public IRepositoryProvider<T> GetRepositoryProvider<T>() where T : class, IAggregate
        {
            return new InMemoryRepositoryProvider<T>();
        }
    }
}