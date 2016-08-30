using System;
using Teclyn.Core.Domains;
using Teclyn.Core.Events;
using Teclyn.SampleCore.TodoLists.Events;

namespace Teclyn.SampleCore.TodoLists.Models
{
    [AggregateImplementation]
    public class TodoList : ITodoList
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Length { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastModificationDate { get; set; }

        public void Create(IEventInformation<TodoListCreatedEvent> eventInformation)
        {
            this.Id = eventInformation.Event.AggregateId;
            this.CreationDate = eventInformation.Date;
            this.LastModificationDate = eventInformation.Date;
            this.Length = 0;
            this.Name = eventInformation.Event.Name;
        }
    }
}