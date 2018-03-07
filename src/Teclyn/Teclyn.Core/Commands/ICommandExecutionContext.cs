using Teclyn.Core.Api;
using Teclyn.Core.Events;
using Teclyn.Core.Ioc;
using Teclyn.Core.Security;

namespace Teclyn.Core.Commands
{
    public interface ICommandExecutionContext
    {
        IDependencyResolver DependencyResolver { get; }
    }
}