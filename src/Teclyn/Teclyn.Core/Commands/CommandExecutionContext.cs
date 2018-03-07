using Teclyn.Core.Api;
using Teclyn.Core.Ioc;

namespace Teclyn.Core.Commands
{
    public class CommandExecutionContext : ICommandExecutionContext
    {
        public CommandExecutionContext(IDependencyResolver dependencyResolver)
        {
            this.DependencyResolver = dependencyResolver;
        }
        
        public IDependencyResolver DependencyResolver { get; }
    }
}