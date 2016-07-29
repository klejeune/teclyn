using System;
using Teclyn.Core.Events;
using Teclyn.Core.Events.Handlers;

namespace Teclyn.Core.Dummies
{
    public class DummyEventHandler : IEventHandler<IDummyAggregate, DummyCreationEvent>
    {
        public void Handle(IDummyAggregate aggregate, IEventInformation<DummyCreationEvent> @event)
        {
            throw new NotImplementedException();
        }
    }
}