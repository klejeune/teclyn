using System;

namespace Teclyn.Sample.Blog.Core.Comments.Events
{
    public class CommentPublishedEvent
    {
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }
        public string AuthorId { get; set; }
        public Guid Id { get; set; }
    }
}