using System;

namespace Teclyn.Core.Security.Context
{
    public class TeclynContextLevel : IDisposable
    {
        public ITeclynUser User { get; }
        private Action onDispose { get; }

        public TeclynContextLevel(ITeclynUser user, Action onDispose)
        {
            this.User = user;
            this.onDispose = onDispose;
        }

        public void Dispose()
        {
            this.onDispose();
        }
    }
}