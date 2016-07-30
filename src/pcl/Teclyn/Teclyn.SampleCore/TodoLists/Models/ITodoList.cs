using System;
using Teclyn.Core.Domains;

namespace Teclyn.SampleCore.TodoLists.Models
{
    [Aggregate]
    public interface ITodoList : IDisplayable, IAggregate
    {
        int Length { get; }
        DateTime CreationDate { get; }
        DateTime LastModificationDate { get; }
    }
}