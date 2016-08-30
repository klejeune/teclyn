using Teclyn.Core.Events;
using Teclyn.SampleCore.Users.Models;

namespace Teclyn.SampleCore.Users.Events
{
    public class UserRegisteredEvent : IEvent<IUser>
    {
        public void Apply(IUser aggregate, IEventInformation information)
        {
            aggregate.Create(information.Type(this));
        }

        public string AggregateId { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
    }
}