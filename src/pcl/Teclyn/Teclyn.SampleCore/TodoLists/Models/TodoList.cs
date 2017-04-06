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

        public void Create(TodoListCreatedEvent eventInformation)
        {
            this.Id = eventInformation.AggregateId;
            this.CreationDate = eventInformation.Date;
            this.LastModificationDate = eventInformation.Date;
            this.Length = 0;
            this.Name = eventInformation.Name;
        }

        public void Rename(TodoListRenamedEvent eventInformation)
        {
            this.Name = eventInformation.NewValue;
        }

        public void AddTodo(TodoAddedToListEvent todoAddedToListEvent)
        {
            this.Length++;
        }
    }
}