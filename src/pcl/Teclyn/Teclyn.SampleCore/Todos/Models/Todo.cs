using System;
using Teclyn.Core.Domains;
using Teclyn.Core.Events;
using Teclyn.SampleCore.Todos.Events;

namespace Teclyn.SampleCore.Todos.Models
{
    [AggregateImplementation]
    public class Todo : ITodo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ITodoTodoList TodoList { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastModificationDate { get; set; }
        public string Text { get; set; }

        public Todo(IEventInformation<TodoCreatedEvent> eventInformation)
        {
            this.Id = eventInformation.Event.AggregateId;
            this.CreationDate = eventInformation.Date;
            this.LastModificationDate = eventInformation.Date;
            this.Text = eventInformation.Event.Text;
            this.Name = eventInformation.Event.Text.Substring(0, 64);
            this.TodoList = new TodoTodoList
            {
                Id = eventInformation.Event.TodoListId,
                Name = eventInformation.Event.TodoListName,
            };
        }
    }
}