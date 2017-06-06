using System.Linq;
using System.Threading.Tasks;
using Teclyn.Core.Commands;
using Teclyn.Core.Domains;

namespace Teclyn.Core.Storage
{
    public interface IRepository<T> : IQueryable<T> where T : class, IAggregate
    {
        Task<T> GetById(Id<T> id);
        Task<T> GetByIdOrNull(Id<T> id);
        Task<bool> Exists(string id);
        Task Create(T item);
        Task Save(T item);
        Task Delete(T item);
    }
}