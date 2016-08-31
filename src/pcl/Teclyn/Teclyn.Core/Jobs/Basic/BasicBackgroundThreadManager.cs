using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Teclyn.Core.Domains;

namespace Teclyn.Core.Jobs.Basic
{
    [ServiceImplementation]
    public class BasicBackgroundThreadManager : IBackgroundThreadManager, IBackgroundThreadState
    {
        private ConcurrentQueue<Action<IBackgroundThreadState>> queue = new ConcurrentQueue<Action<IBackgroundThreadState>>();

        private bool mustStop;
        private EventWaitHandle waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
        private TimeSpan waitTimeout = TimeSpan.FromSeconds(1);

        public void Start()
        {
            Task.Run(() =>
            {
                while (!mustStop)
                {
                    Action<IBackgroundThreadState> nextAction;
                    waitHandle.Reset();

                    if (queue.TryDequeue(out nextAction))
                    {
                        nextAction.Invoke(this);
                    }
                    else
                    {
                        waitHandle.WaitOne(waitTimeout);
                    }
                }
            });
        }

        public void Stop()
        {
            this.mustStop = true;
            this.waitHandle.Set();
        }

        public void Queue(Action<IBackgroundThreadState> action)
        {
            this.queue.Enqueue(action);
            this.waitHandle.Set();
        }

        public bool MustStop => this.mustStop;
    }
}