using System.Linq;

namespace Teclyn.Core.Storage
{
    public interface IRepositoryProvider<T> : IQueryable<T> where T : class, IIndexable
    {
        T GetByIdOrNull(string id);
        void Create(T item);
        void Save(T item);
        void Delete(string id);
    }
}