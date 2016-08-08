using System.Linq;
using Teclyn.Core.Domains;

namespace Teclyn.Core.Storage
{
    public interface IRepositoryProvider<T> : IQueryable<T> where T : class, IAggregate
    {
        T GetByIdOrNull(string id);
        void Create(T item);
        void Save(T item);
        void Delete(T item);
    }
}