using System;
using Teclyn.Core.Domains;
using Teclyn.Sample.Blog.Core.Users.Events;

namespace Teclyn.Sample.Blog.Core.Users.Models
{
    public interface IUser : IAggregate
    {
        string EmailAddress { get; }
        DateTime RegistrationDate { get; }
        void Register(Registered registered);
    }
}