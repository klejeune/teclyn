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

        public void Create(UserRegisteredEvent @event)
        {
            this.Id = @event.AggregateId;
            this.Name = @event.Fullname;
            this.Email = @event.Email;
            this.RegistrationDate = @event.Date;
        }
    }
}