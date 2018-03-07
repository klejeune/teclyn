using System;
using Teclyn.Core.Domains;
using Teclyn.Core.Events;
using Teclyn.SampleCore.Users.Events;

namespace Teclyn.SampleCore.Users.Models
{
    [Aggregate]
    public interface IUser : IDisplayable, IAggregate
    {
        string Email { get; }
        DateTime RegistrationDate { get; }
        void Create(UserRegisteredEvent @event);
    }
}