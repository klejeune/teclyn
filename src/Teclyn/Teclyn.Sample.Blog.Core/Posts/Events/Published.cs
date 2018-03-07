using System;
using Teclyn.Core.Events;
using Teclyn.Sample.Blog.Core.Posts.Models;

namespace Teclyn.Sample.Blog.Core.Posts.Events
{
    public class Published : IEvent<IPost>
    {
        public DateTime PublicationDate { get; set; }

        public void Apply(IPost aggregate)
        {
            aggregate.Publish(this);
        }

        public string AggregateId { get; set; }
    }
}