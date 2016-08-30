using Teclyn.Core.Events;
using Teclyn.SampleCore.TodoLists.Models;

namespace Teclyn.SampleCore.TodoLists.Events
{
    public class TodoListCreatedEvent : ICreationEvent<ITodoList>
    {
        public string Name { get; set; }

        public void Apply(ITodoList aggregate, IEventInformation information)
        {
            aggregate.Create(information.Type(this));
        }

        public string AggregateId { get; set; }
    }
}