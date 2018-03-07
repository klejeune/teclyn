using System;
using Teclyn.Core.Domains;
using Teclyn.Core.Errors.Events;
using Teclyn.Core.Events;

namespace Teclyn.Core.Errors.Models
{
    [Aggregate]
    public interface IError : IAggregate, IDisplayable
    {
        DateTime Date { get; }
        string Description { get; }
        void Create(ErrorLoggedEvent @event);
    }
}