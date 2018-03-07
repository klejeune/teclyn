using System.Threading.Tasks;
using Teclyn.Core.Commands;
using Teclyn.Core.Security.Context;

namespace Teclyn.Sample.Blog.Core.Posts.Commands
{
    public class UpdatePostTitleCommandHandler : ICommandHandler<UpdatePostTitle>
    {
        public Task<bool> CheckParameters(UpdatePostTitle command, IParameterChecker _)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> CheckContext(UpdatePostTitle command, ITeclynContext context, ICommandContextChecker _)
        {
            throw new System.NotImplementedException();
        }

        public Task Execute(UpdatePostTitle command, ICommandExecutionContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}