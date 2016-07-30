using Teclyn.Core.Configuration;
using Teclyn.Core.Security.Context;

namespace Teclyn.Core.Tests
{
    public class TestEnvironment : IEnvironment
    {
        private class TeclynUser : ITeclynUser
        {
            public string Id { get { return "test"; } }
            public string Name { get { return "Test User"; } }
            public bool IsAdmin => false;
            public bool IsGuest => false;
        }

        private static ITeclynUser user = new TeclynUser();

        public ITeclynUser GetCurrentUser()
        {
            return user;
        }
    }
}