using Teclyn.Core.Events;
using Teclyn.Core.Security;

namespace Teclyn.Core.Commands
{
    public interface ICommandExecutionContext
    {
        TeclynApi Teclyn { get; }
    }
}