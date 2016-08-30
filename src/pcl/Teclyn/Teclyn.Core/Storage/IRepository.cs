using System.Linq;
using Teclyn.Core.Domains;

namespace Teclyn.Core.Storage
{
    public interface IRepository<T> : IQueryable<T> where T : class, IAggregate
    {
        T GetById(string id);
        T GetByIdOrNull(string id);
        bool Exists(string id);
        void Create(T item);
        void Save(T item);
        void Delete(T item);
    }
}