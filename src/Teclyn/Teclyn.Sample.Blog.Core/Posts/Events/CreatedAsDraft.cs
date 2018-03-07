using Teclyn.Core.Events;
using Teclyn.Sample.Blog.Core.Posts.Models;

namespace Teclyn.Sample.Blog.Core.Posts.Events
{
    public class CreatedAsDraft : IEvent<IPost>
    {
        public string Title { get; set; }

        public void Apply(IPost aggregate)
        {
            aggregate.CreateDraft(this);
        }

        public string AggregateId { get; set; }
    }
}