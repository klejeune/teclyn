using System.Linq;

namespace Teclyn.Core.Domains
{
    public interface IFilter<TAggregate>
    {
        IQueryable<TAggregate> Filter(IQueryable<TAggregate> query);
    }
}