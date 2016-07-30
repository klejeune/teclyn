using Teclyn.Core.Configuration;
using Teclyn.Core.Ioc;
using Teclyn.Core.Security.Context;

namespace Teclyn.Core.Services
{
    public class UserContext
    {
        [Inject]
        public IEnvironment Environment { get; set; }

        public UserContext(IEnvironment environment)
        {
            this.Environment = environment;
        }

        public ITeclynUser User => this.Environment.GetCurrentUser();
    }
}