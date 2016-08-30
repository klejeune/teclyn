using Teclyn.Core.Domains;
using Teclyn.Core.Events;

namespace Teclyn.Core.Dummies
{
    public interface IDummyAggregate : IAggregate
    {
        void Create(IEventInformation<DummyCreationEvent> eventInformation);
    }
}