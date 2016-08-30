using System;
using Teclyn.Core.Domains;
using Teclyn.Core.Events;
using Teclyn.SampleCore.Todos.Events;

namespace Teclyn.SampleCore.Todos.Models
{
    [Aggregate]
    public interface ITodo : IDisplayable, IAggregate
    {
        ITodoTodoList TodoList { get; }
        DateTime CreationDate { get; }
        DateTime LastModificationDate { get; }
        string Text { get; }
        void Create(IEventInformation<TodoCreatedEvent> eventInformation);
    }
}