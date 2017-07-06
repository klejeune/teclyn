using System;
using Teclyn.Core.Domains;

namespace Teclyn.Core.Services
{
    [Service]
    public interface ITimeService
    {
        DateTime Now();
    }
}