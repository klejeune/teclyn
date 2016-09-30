using System;
using System.Threading.Tasks;
using Teclyn.Core.Events;
using Teclyn.Core.Events.Handlers;

namespace Teclyn.Core.Dummies
{
    public class DummyEventHandler : IEventHandler<IDummyAggregate, DummyCreationEvent>
    {
        public Task Handle(IDummyAggregate aggregate, IEventInformation<DummyCreationEvent> @event)
        {
            throw new NotImplementedException();
        }
    }
}