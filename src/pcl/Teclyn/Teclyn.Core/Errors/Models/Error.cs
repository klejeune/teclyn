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
        public void Create(IEventInformation<ErrorLoggedEvent> type)
        {
            this.Id = type.Event.AggregateId;
            this.Name = type.Event.Message;
            this.Description = type.Event.Description;
            this.Date = type.Date;
        }

        public string Name { get; set; }
    }
}