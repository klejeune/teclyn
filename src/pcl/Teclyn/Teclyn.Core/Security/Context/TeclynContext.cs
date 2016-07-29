using System;
using System.Collections.Generic;

namespace Teclyn.Core.Security.Context
{
    public class TeclynContext : ITeclynContext
    {
        private static readonly ITeclynUser guestUser = new GuestTeclynUser();
        private static readonly ITeclynUser technicalUser = new TechnicalUser();

        public ITeclynUser CurrentUser => this.contextLevels.Peek().User;

        private Stack<TeclynContextLevel> contextLevels = new Stack<TeclynContextLevel>();

        public TeclynContext()
        {
            this.contextLevels.Push(new TeclynContextLevel(guestUser,
                () => { throw new TeclynSecurityException("The base context cannot be exited."); }));
        }
        public IDisposable NewContext(ITeclynUser user)
        {
            var newContext = new TeclynContextLevel(user, () => this.contextLevels.Pop());

            this.contextLevels.Push(newContext);

            return newContext;
        }

        public IDisposable TechnicalContext()
        {
            return this.NewContext(technicalUser);
        }
    }
}