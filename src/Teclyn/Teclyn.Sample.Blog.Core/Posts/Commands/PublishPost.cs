using System.Threading.Tasks;
using Teclyn.Core.Commands;
using Teclyn.Core.Security.Context;
using Teclyn.Sample.Blog.Core.Posts.Events;

namespace Teclyn.Sample.Blog.Core.Posts.Commands
{
    public class PublishPost : ICommand
    {
        public string PostId { get; set; }

        public bool CheckParameters(IParameterChecker _)
        {
            return true;
        }

        public bool CheckContext(ITeclynContext context, ICommandContextChecker _)
        {
            return true;
        }

        public async Task Execute(ICommandExecutionContext context)
        {
            await context.GetEventService().Raise(new Published
            {
                AggregateId = this.PostId,
                PublicationDate = context.GetTimeService().Now(),
            });
        }
    }
}