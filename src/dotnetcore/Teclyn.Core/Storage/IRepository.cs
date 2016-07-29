using System.Linq;

namespace Teclyn.Core.Storage
{
    public interface IRepository<T> : IQueryable<T> where T : class, IIndexable
    {
        T GetById(string id);
        T GetByIdOrNull(string id);
        void Create(T item);
        void Save(T item);
        void Delete(string id);
    }
}