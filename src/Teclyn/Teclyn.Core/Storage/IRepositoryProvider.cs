using System.Linq;
using System.Threading.Tasks;
using Teclyn.Core.Commands;
using Teclyn.Core.Domains;

namespace Teclyn.Core.Storage
{
    public interface IRepositoryProvider<T> : IQueryable<T> where T : IAggregate
    {
        Task<T> GetByIdOrNull(Id<T> id);
        Task Create(T item);
        Task Save(T item);
        Task Delete(T item);
        Task<bool> Exists(string id);
    }
}