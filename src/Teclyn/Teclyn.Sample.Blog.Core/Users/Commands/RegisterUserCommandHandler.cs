using System.Threading.Tasks;
using Teclyn.Core.Commands;
using Teclyn.Core.Security.Context;
using Teclyn.Sample.Blog.Core.Users.Events;

namespace Teclyn.Sample.Blog.Core.Users.Commands
{
    public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand>
    {
        public Task<bool> CheckParameters(RegisterUserCommand command, IParameterChecker _)
        {
            return Task.FromResult(true);
        }

        public Task<bool> CheckContext(RegisterUserCommand command, ITeclynContext context, ICommandContextChecker _)
        {
            return Task.FromResult(true);
        }

        public async Task Execute(RegisterUserCommand command, ICommandExecutionContext context)
        {
            await context.GetEventService().Raise(new Registered
            {
                AggregateId = command.Username,
                EmailAddress = command.EmailAddress,
                RegistrationDate = context.GetTimeService().Now(),
            });
        }
    }
}