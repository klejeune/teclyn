using Teclyn.Core.Events;
using Teclyn.SampleCore.TodoLists.Models;

namespace Teclyn.SampleCore.TodoLists.Events
{
    public class TodoAddedToListEvent : IEvent<ITodoList>
    {
        public void Apply(ITodoList aggregate)
        {
            aggregate.AddTodo(this);
        }

        public string AggregateId { get; set; }
    }
}