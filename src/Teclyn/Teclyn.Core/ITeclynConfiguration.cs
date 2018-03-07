using System.Collections;
using System.Collections.Generic;
using Teclyn.Core.Ioc;
using Teclyn.Core.Storage;

namespace Teclyn.Core
{
    public interface ITeclynConfiguration
    {
        IDependencyResolver DependencyResolver { get; }
        IStorageConfiguration StorageConfiguration { get; }
        bool Debug { get; }
    }
}