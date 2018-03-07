using Teclyn.Core.Commands;
using Teclyn.Core.Domains;
using Teclyn.Core.Events;

namespace Teclyn.Core.Api
{
    public interface ITeclynAggregateApi<TDomain, in TAggregate>
        where TDomain : IDomain
        where TAggregate : IAggregate
    {
        ITeclynAggregateApi<TDomain, TAggregate> AddEvent<TEvent>() where TEvent : ITeclynEvent<TAggregate>;
    }
}