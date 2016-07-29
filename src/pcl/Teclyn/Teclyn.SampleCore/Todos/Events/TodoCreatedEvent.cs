using Teclyn.Core.Events;
using Teclyn.SampleCore.Todos.Models;

namespace Teclyn.SampleCore.Todos.Events
{
    public class TodoCreatedEvent : ICreationEvent<ITodo>
    {
        public string Text { get; set; }
        public string TodoListId { get; set; }
        public string TodoListName { get; set; }

        public ITodo Apply(IEventInformation information)
        {
            return new Todo(information.Type(this));
        }

        public string AggregateId { get; set; }
    }
}