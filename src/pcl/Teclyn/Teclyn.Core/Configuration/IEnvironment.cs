using Teclyn.Core.Security.Context;

namespace Teclyn.Core.Configuration
{
    public interface IEnvironment
    {
        ITeclynUser GetCurrentUser();
    }
}