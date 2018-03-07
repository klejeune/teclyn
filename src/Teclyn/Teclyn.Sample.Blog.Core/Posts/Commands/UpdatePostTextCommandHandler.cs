using System.Threading.Tasks;
using Teclyn.Core.Commands;
using Teclyn.Core.Security.Context;

namespace Teclyn.Sample.Blog.Core.Posts.Commands
{
    public class UpdatePostTextCommandHandler : ICommandHandler<UpdatePostText>
    {
        public Task<bool> CheckParameters(UpdatePostText command, IParameterChecker _)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> CheckContext(UpdatePostText command, ITeclynContext context, ICommandContextChecker _)
        {
            throw new System.NotImplementedException();
        }

        public Task Execute(UpdatePostText command, ICommandExecutionContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}