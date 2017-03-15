using System;
using Teclyn.Core.Events;
using Teclyn.SampleCore.TodoLists.Models;

namespace Teclyn.SampleCore.TodoLists.Events
{
    public class TodoListCreatedEvent : IEvent<ITodoList>
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }

        public void Apply(ITodoList aggregate)
        {
            aggregate.Create(this);
        }

        public string AggregateId { get; set; }
    }
}