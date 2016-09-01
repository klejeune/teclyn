using System;
using System.Collections.Generic;
using Teclyn.Core.Domains;
using Teclyn.Core.Jobs;

namespace Teclyn.Core.Jobs
{
    [Service]
    public interface IBackgroundThreadManager
    {
        void Start();
        void Stop();
        void Queue(string name, Action<IBackgroundThreadState> action);
        IEnumerable<IBackgroundThread> Threads { get; }
    }
}