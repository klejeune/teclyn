using System;
using Teclyn.Core.Domains;
using Teclyn.Core.Jobs;

namespace Teclyn.Core.Jobs
{
    [Service]
    public interface IBackgroundThreadManager
    {
        void Start();
        void Stop();
        void Queue(Action<IBackgroundThreadState> action);
    }
}