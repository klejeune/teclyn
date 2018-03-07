using System;
using Teclyn.Core.Domains;

namespace Teclyn.Core.Jobs
{
    public interface IBackgroundThread : IDisplayable
    {
        DateTime CreationDate { get; }
        DateTime? StartDate { get; }
        DateTime? EndDate { get; }
        TimeSpan? GetDuration(DateTime now);
        ThreadState State { get; }
        void Start();
        void Stop();
    }
}