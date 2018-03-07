using System;
using Teclyn.Core.Events;
using Teclyn.SampleCore.Users.Models;

namespace Teclyn.SampleCore.Users.Events
{
    public class UserRegisteredEvent : IEvent<IUser>
    {
        public void Apply(IUser aggregate)
        {
            aggregate.Create(this);
        }

        public string AggregateId { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public DateTime Date { get; set; }
    }
}