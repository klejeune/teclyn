using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Teclyn.Core.Basic;
using Teclyn.Core.Ioc;
using Teclyn.Core.Storage;

namespace Teclyn.Core
{
    public class TeclynDefaultConfiguration : ITeclynConfiguration
    {
        public IDependencyResolver DependencyResolver => null;
        public IStorageConfiguration StorageConfiguration => new BasicStorageConfiguration();
        public bool Debug => false;
    }
}