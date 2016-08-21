using System.Collections.Generic;
using StructureMap;
using Teclyn.Core.Basic;
using Teclyn.Core.Domains;
using Teclyn.Core.Ioc;
using Teclyn.Core.Services;
using Teclyn.Core.Storage;
using Teclyn.StructureMap;

namespace Teclyn.Core.Tests
{
    public class TeclynTestConfiguration : ITeclynConfiguration
    {
        public StructureMapContainer TestIocContainer { get; }

        public IIocContainer IocContainer { get { return this.TestIocContainer; } }
        public IStorageConfiguration StorageConfiguration { get; }

        public IEnumerable<ITeclynPlugin> Plugins => new ITeclynPlugin[]
        {
            new TeclynCorePlugin(), 
            new TeclynStructureMapPlugin(),
            new TeclynCoreTestsPlugin(),
        };

        public bool Debug => false;

        public TeclynTestConfiguration()
        {
            this.TestIocContainer = new StructureMapContainer(new Container());
        }
    }
}