using System;
using Teclyn.Sample.Blog.Core.Comments.Events;

namespace Teclyn.Sample.Blog.Core.Comments.Models
{
    public class Comment : IComment
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }
        public string AuthorId { get; set; }

        public void Create(CommentPublishedEvent @event)
        {
            this.Id = @event.Id;
            this.Content = @event.Content;
            this.CreationDate = @event.CreationDate;
            this.AuthorId = @event.AuthorId;
        }
    }
}