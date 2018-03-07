using Teclyn.Core.Events;
using Teclyn.Sample.Blog.Core.Posts.Models;

namespace Teclyn.Sample.Blog.Core.Posts.Events
{
    public class TitleUpdated : IEvent<IPost>
    {
        public string Title { get; set; }

        public void Apply(IPost aggregate)
        {
            aggregate.UpdateTitle(this);
        }

        public string AggregateId { get; set; }
    }
}