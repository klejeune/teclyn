using System.Threading.Tasks;
using Teclyn.Core.Commands;
using Teclyn.Core.Security.Context;

namespace Teclyn.Sample.Blog.Core.Posts.Commands
{
    public class PublishPostCommandHandler : ICommandHandler<PublishPost>
    {
        public Task<bool> CheckParameters(PublishPost command, IParameterChecker _)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> CheckContext(PublishPost command, ITeclynContext context, ICommandContextChecker _)
        {
            throw new System.NotImplementedException();
        }

        public Task Execute(PublishPost command, ICommandExecutionContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}