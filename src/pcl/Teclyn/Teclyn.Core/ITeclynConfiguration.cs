using System.Collections;
using System.Collections.Generic;
using Teclyn.Core.Ioc;
using Teclyn.Core.Storage;

namespace Teclyn.Core
{
    public interface ITeclynConfiguration
    {
        IIocContainer IocContainer { get; }
        IStorageConfiguration StorageConfiguration { get; }
        IEnumerable<ITeclynPlugin> Plugins { get; }
        bool Debug { get; }
    }
}