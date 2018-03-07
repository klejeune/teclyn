using System.Threading.Tasks;
using Teclyn.Core.Commands;
using Teclyn.Core.Security.Context;

namespace Teclyn.Sample.Blog.Core.Posts.Commands
{
    public class CreatePostAsDraftCommandHandler : ICommandHandler<CreatePostAsDraft>
    {
        public Task<bool> CheckParameters(CreatePostAsDraft command, IParameterChecker _)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> CheckContext(CreatePostAsDraft command, ITeclynContext context, ICommandContextChecker _)
        {
            throw new System.NotImplementedException();
        }

        public Task Execute(CreatePostAsDraft command, ICommandExecutionContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}