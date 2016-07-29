using System;
using Teclyn.Core.Domains;

namespace Teclyn.SampleCore.Users.Models
{
    [Aggregate]
    public interface IUser : IDisplayable, IAggregate
    {
        string Email { get; }
        DateTime RegistrationDate { get; } 
    }
}