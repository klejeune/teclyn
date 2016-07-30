using Teclyn.Core.Domains;

namespace Teclyn.Core.Storage
{
    public interface IStorageConfiguration
    {
        IRepositoryProvider<T> GetRepositoryProvider<T>() where T : class, IAggregate;
    }
}