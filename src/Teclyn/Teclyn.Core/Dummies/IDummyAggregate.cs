using Teclyn.Core.Domains;
using Teclyn.Core.Events;

namespace Teclyn.Core.Dummies
{
    public interface IDummyAggregate : IAggregate
    {
        string Property { get; }
        void Create(DummyCreationEvent eventInformation);
        void Update(DummyPropertyEvent eventInformation);
    }
}