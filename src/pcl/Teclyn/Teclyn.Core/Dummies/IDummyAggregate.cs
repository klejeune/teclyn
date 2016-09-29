using Teclyn.Core.Domains;
using Teclyn.Core.Events;

namespace Teclyn.Core.Dummies
{
    public interface IDummyAggregate : IAggregate
    {
        string Property { get; }
        void Create(IEventInformation<DummyCreationEvent> eventInformation);
        void Update(IEventInformation<DummyPropertyEvent> eventInformation);
    }
}