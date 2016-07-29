using System;
using Teclyn.Core.Domains;

namespace Teclyn.SampleCore.Todos.Models
{
    [Aggregate]
    public interface ITodo : IDisplayable, IAggregate
    {
        ITodoTodoList TodoList { get; }
        DateTime CreationDate { get; }
        DateTime LastModificationDate { get; }
        string Text { get; }
    }
}