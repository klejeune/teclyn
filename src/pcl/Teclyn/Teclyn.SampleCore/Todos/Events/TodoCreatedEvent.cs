using Teclyn.Core.Events;
using Teclyn.SampleCore.Todos.Models;

namespace Teclyn.SampleCore.Todos.Events
{
    public class TodoCreatedEvent : ICreationEvent<ITodo>
    {
        public string Text { get; set; }
        public string TodoListId { get; set; }
        public string TodoListName { get; set; }
        
        public void Apply(ITodo aggregate, IEventInformation information)
        {
            aggregate.Create(information.Type(this));
        }

        public string AggregateId { get; set; }
    }
}