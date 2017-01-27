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
        public IIocContainer IocContainer => new BasicIocContainer();
        public IStorageConfiguration StorageConfiguration => new BasicStorageConfiguration();
        public IEnumerable<ITeclynPlugin> Plugins => Enumerable.Empty<ITeclynPlugin>();
        public bool Debug => false;
    }
}