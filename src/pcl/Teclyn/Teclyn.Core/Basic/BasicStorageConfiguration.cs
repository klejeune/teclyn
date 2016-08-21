using Teclyn.Core.Domains;
using Teclyn.Core.Storage;

namespace Teclyn.Core.Basic
{
    public class BasicStorageConfiguration : IStorageConfiguration
    {
        public IRepositoryProvider<T> GetRepositoryProvider<T>(string collectionName) where T : class, IAggregate
        {
            return new InMemoryRepositoryProvider<T>();
        }
    }
}