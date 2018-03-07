using System;
using Teclyn.Core.Events;
using Teclyn.Sample.Blog.Core.Users.Models;

namespace Teclyn.Sample.Blog.Core.Users.Events
{
    public class Registered : IEvent<IUser>
    {
        public string AggregateId { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string EmailAddress { get; set; }

        public void Apply(IUser aggregate)
        {
            aggregate.Register(this);
        }
    }
}