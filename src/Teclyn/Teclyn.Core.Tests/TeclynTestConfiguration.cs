using System.Collections.Generic;
using Teclyn.Core.Basic;
using Teclyn.Core.Domains;
using Teclyn.Core.Ioc;
using Teclyn.Core.Services;
using Teclyn.Core.Storage;

namespace Teclyn.Core.Tests
{
    public class TeclynTestConfiguration : ITeclynConfiguration
    {
        //public StructureMapContainer TestIocContainer { get; }

        //public IDependencyResolver DependencyResolver { get { return this.TestIocContainer; } }
        public IDependencyResolver DependencyResolver { get { return null; } }
        public IStorageConfiguration StorageConfiguration { get; }
        
        public bool Debug => false;

        public TeclynTestConfiguration()
        {
            //this.TestIocContainer = new StructureMapContainer(new Container());
        }
    }
}