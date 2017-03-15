using System;
using Teclyn.Core.Domains;

namespace Teclyn.Core.Services
{
    public class TimeService : ITimeService
    {
        public DateTime Now()
        {
            return DateTime.Now;
        }
    }
}