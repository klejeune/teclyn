using System;
using Teclyn.Core.Domains;
using Teclyn.Core.Errors.Events;
using Teclyn.Core.Events;

namespace Teclyn.Core.Errors.Models
{
    [AggregateImplementation]
    public class Error : IError
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public void Create(ErrorLoggedEvent @event)
        {
            this.Id = @event.AggregateId;
            this.Name = @event.Message;
            this.Description = @event.Description;
            this.Date = @event.Date;
        }

        public string Name { get; set; }
    }
}