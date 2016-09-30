using System.Linq;
using System.Threading.Tasks;
using Teclyn.Core.Domains;

namespace Teclyn.Core.Storage
{
    public interface IRepository<T> : IQueryable<T> where T : class, IAggregate
    {
        Task<T> GetById(string id);
        Task<T> GetByIdOrNull(string id);
        Task<bool> Exists(string id);
        Task Create(T item);
        Task Save(T item);
        Task Delete(T item);
    }
}