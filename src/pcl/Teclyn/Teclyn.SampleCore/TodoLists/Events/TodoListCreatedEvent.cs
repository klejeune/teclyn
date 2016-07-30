using Teclyn.Core.Events;
using Teclyn.SampleCore.TodoLists.Models;

namespace Teclyn.SampleCore.TodoLists.Events
{
    public class TodoListCreatedEvent : ICreationEvent<ITodoList>
    {
        public string Name { get; set; }

        public ITodoList Apply(IEventInformation information)
        {
            return new TodoList(information.Type(this));
        }

        public string AggregateId { get; set; }
    }
}