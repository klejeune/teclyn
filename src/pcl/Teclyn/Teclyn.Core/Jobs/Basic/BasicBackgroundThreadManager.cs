using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Teclyn.Core.Domains;
using Teclyn.Core.Ioc;
using Teclyn.Core.Services;
using Teclyn.Core.Tools;

namespace Teclyn.Core.Jobs.Basic
{
    [ServiceImplementation]
    public class BasicBackgroundThreadManager : IBackgroundThreadManager
    {
        [Inject]
        public IdGenerator IdGenerator { get; set; }

        [Inject]
        public TimeService TimeService { get; set; }
        
        private readonly IDictionary<string, IBackgroundThread> threads = new ConcurrentDictionary<string, IBackgroundThread>();

        private bool mustStop;
        private EventWaitHandle waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
        private TimeSpan waitTimeout = TimeSpan.FromSeconds(1);
        private const int maxRunningThreads = 10;

        public void Start()
        {
            Task.Run(() =>
            {
                while (!mustStop)
                {
                    this.CleanThreads();

                    waitHandle.Reset();

                    var runningThreads = this.threads.Values.Where(t => t.State == ThreadState.Running).ToList();
                    var waitingThreads = this.threads.Values.Where(t => t.State == ThreadState.Waiting).ToList();

                    if (runningThreads.Count < maxRunningThreads && waitingThreads.Count > 0)
                    {
                        var threadsToLaunch = waitingThreads.Take(maxRunningThreads - runningThreads.Count).ToList();

                        foreach (var thread in threadsToLaunch)
                        {
                            thread.Start();
                        }
                    }
                    else
                    {
                        waitHandle.WaitOne(waitTimeout);
                    }
                }
            });
        }

        public IEnumerable<IBackgroundThread> Threads => this.threads.Values;

        private void CleanThreads()
        {
            var threadsToClean = this.threads.Where(threadPair => threadPair.Value.State == ThreadState.Finished).ToList();

            foreach (var thread in threadsToClean)
            {
                this.threads.Remove(thread);
            }
        }

        public void Stop()
        {
            foreach (var thread in this.threads.Values.Where(t => t.State == ThreadState.Running))
            {
                thread.Stop();
            }

            this.mustStop = true;
            this.waitHandle.Set();
        }

        public void Queue(string name, Action<IBackgroundThreadState> action)
        {
            var thread = new BackgroundThread(this.TimeService, this.IdGenerator.GenerateId(), name, action, () => this.waitHandle.Set());

            this.threads.Add(thread.Id, thread);
            this.waitHandle.Set();
        }

        public bool MustStop => this.mustStop;
    }
}