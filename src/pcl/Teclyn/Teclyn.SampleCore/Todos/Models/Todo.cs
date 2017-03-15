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

        public void Create(TodoCreatedEvent eventInformation)
        {
            this.Id = eventInformation.AggregateId;
            this.CreationDate = eventInformation.Date;
            this.LastModificationDate = eventInformation.Date;
            this.Text = eventInformation.Text;
            this.Name = eventInformation.Text?.Substring(0, Math.Min(eventInformation.Text.Length, 64)) ?? string.Empty;
            this.TodoList = new TodoTodoList
            {
                Id = eventInformation.TodoListId,
                Name = eventInformation.TodoListName,
            };
        }

        public void UpdateText(TodoTextUpdatedEvent eventInformation)
        {
            this.Text = eventInformation.NewValue;
        }
    }
}