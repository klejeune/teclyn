using System;
using System.Threading.Tasks;
using Teclyn.Core.Services;

namespace Teclyn.Core.Jobs.Basic
{
    public class BackgroundThread : IBackgroundThread, IBackgroundThreadState
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; }
        public DateTime? StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }
        public TimeSpan? GetDuration(DateTime now)
        {
            if (this.StartDate.HasValue)
            {
                return this.time.Now - this.StartDate.Value;
            }
            else
            {
                return null;
            }
        }

        public ThreadState State { get; private set; }
        public bool MustStop { get; private set; }

        private readonly Action<IBackgroundThreadState> action;
        private readonly Action onFinished;
        private readonly Time time;
        
        public BackgroundThread(Time time, string id, string name, Action<IBackgroundThreadState> action, Action onFinished)
        {
            this.time = time;
            this.Id = id;
            this.Name = name;
            this.action = action;
            this.onFinished = onFinished;
            this.CreationDate = this.time.Now;
        }

        public void Start()
        {
            Task.Run(() =>
            {
                this.StartDate = this.time.Now;
                this.State = ThreadState.Running;
                action(this);
                this.State = ThreadState.Finished;
                this.EndDate = this.time.Now;
                this.onFinished();
            });
        }

        public void Stop()
        {
            this.MustStop = true;
        }
    }
}