using System;
using Teclyn.Core.Domains;
using Teclyn.Core.Events;
using Teclyn.SampleCore.Users.Events;

namespace Teclyn.SampleCore.Users.Models
{
    [AggregateImplementation]
    public class User : IUser
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime RegistrationDate { get; set; }
        public void Create(IEventInformation<UserRegisteredEvent> eventInformation)
        {
            this.Id = eventInformation.Event.AggregateId;
            this.Name = eventInformation.Event.Fullname;
            this.Email = eventInformation.Event.Email;
            this.RegistrationDate = eventInformation.Date;
        }
    }
}