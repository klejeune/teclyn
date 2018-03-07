using Teclyn.Core.Domains;

namespace Teclyn.Core.Storage
{
    public interface IStorageConfiguration
    {
        IRepositoryProvider<TInterface> GetRepositoryProvider<TInterface, TImplementation>(string collectionName) 
            where TInterface : class, IAggregate
            where TImplementation : TInterface;
    }
}