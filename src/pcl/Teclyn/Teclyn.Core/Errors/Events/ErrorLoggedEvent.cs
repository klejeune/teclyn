using Teclyn.Core.Errors.Models;
using Teclyn.Core.Events;

namespace Teclyn.Core.Errors.Events
{
    public class ErrorLoggedEvent : IEvent<IError>
    {
        public string Message { get; set; }
        public string Description { get; set; }

        public void Apply(IError aggregate, IEventInformation information)
        {
            aggregate.Create(information.Type(this));
        }

        public string AggregateId { get; set; }
    }
}