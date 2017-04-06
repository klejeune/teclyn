using System;
using Teclyn.Core.Domains;
using Teclyn.Core.Events;
using Teclyn.SampleCore.TodoLists.Events;

namespace Teclyn.SampleCore.TodoLists.Models
{
    [Aggregate]
    public interface ITodoList : IDisplayable, IAggregate
    {
        int Length { get; }
        DateTime CreationDate { get; }
        DateTime LastModificationDate { get; }
        void Create(TodoListCreatedEvent eventInformation);
        void Rename(TodoListRenamedEvent eventInformation);
        void AddTodo(TodoAddedToListEvent todoAddedToListEvent);
    }
}