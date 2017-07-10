using Teclyn.Core.Domains;
using Teclyn.Core.Storage;

namespace Teclyn.Core.Basic
{
    public class BasicStorageConfiguration : IStorageConfiguration
    {
        public IRepositoryProvider<TInterface> GetRepositoryProvider<TInterface, TImplementation>(string collectionName) 
            where TInterface : class, IAggregate
            where TImplementation : TInterface
        {
            return new InMemoryRepositoryProvider<TInterface>();
        }
    }
}