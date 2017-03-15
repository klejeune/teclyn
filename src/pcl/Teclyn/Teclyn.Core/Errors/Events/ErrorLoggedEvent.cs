using System;
using Teclyn.Core.Errors.Models;
using Teclyn.Core.Events;

namespace Teclyn.Core.Errors.Events
{
    public class ErrorLoggedEvent : IEvent<IError>
    {
        public string Message { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }

        public void Apply(IError aggregate)
        {
            aggregate.Create(this);
        }

        public string AggregateId { get; set; }
    }
}