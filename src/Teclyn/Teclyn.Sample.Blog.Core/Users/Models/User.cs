using System;
using Teclyn.Sample.Blog.Core.Users.Events;

namespace Teclyn.Sample.Blog.Core.Users.Models
{
    public class User : IUser
    {
        public string Id { get; set; }
        public string EmailAddress { get; set; }
        public DateTime RegistrationDate { get; set; }

        public void Register(Registered registered)
        {
            this.Id = registered.AggregateId;
            this.EmailAddress = registered.EmailAddress;
            this.RegistrationDate = registered.RegistrationDate;
        }
    }
}