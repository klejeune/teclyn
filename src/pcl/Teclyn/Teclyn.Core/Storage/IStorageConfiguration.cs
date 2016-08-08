using Teclyn.Core.Domains;

namespace Teclyn.Core.Storage
{
    public interface IStorageConfiguration
    {
        IRepositoryProvider<T> GetRepositoryProvider<T>(string collectionName) where T : class, IAggregate;
    }
}