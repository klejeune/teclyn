using Teclyn.Core.Events;
using Teclyn.Sample.Blog.Core.Posts.Models;

namespace Teclyn.Sample.Blog.Core.Posts.Events
{
    public class TextUpdated : IEvent<IPost>
    {
        public string Text { get; set; }

        public void Apply(IPost aggregate)
        {
            aggregate.UpdateText(this);
        }

        public string AggregateId { get; set; }
    }
}