using Teclyn.Core.Events;
using Teclyn.SampleCore.Users.Models;

namespace Teclyn.SampleCore.Users.Events
{
    public class UserRegisteredEvent : ICreationEvent<IUser>
    {
        public string AggregateId { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        
        public IUser Apply(IEventInformation information)
        {
           return new User(information.Type(this));
        }
    }
}