using System;
using Teclyn.Core.Events;
using Teclyn.SampleCore.Todos.Models;

namespace Teclyn.SampleCore.Todos.Events
{
    public class TodoCreatedEvent : IEvent<ITodo>
    {
        public string Text { get; set; }
        public string TodoListId { get; set; }
        public string TodoListName { get; set; }
        public DateTime Date { get; set; }

        public void Apply(ITodo aggregate)
        {
            aggregate.Create(this);
        }

        public string AggregateId { get; set; }
    }
}