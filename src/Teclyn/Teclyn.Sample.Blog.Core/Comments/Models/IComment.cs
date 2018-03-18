using System;

namespace Teclyn.Sample.Blog.Core.Comments.Models
{
    public interface IComment
    {
        Guid Id { get; }
        string Content { get; }
        DateTime CreationDate { get; }
        string AuthorId { get; }
    }
}