using System;
using Teclyn.Core.Domains;
using Teclyn.Sample.Blog.Core.Posts.Events;

namespace Teclyn.Sample.Blog.Core.Posts.Models
{
    public interface IPost : IAggregate
    {
        string Title { get; }
        string Text { get; }
        DateTime CreationDate { get; }
        DateTime? PublicationDate { get; }
        bool IsDraft { get; }

        void CreateDraft(CreatedAsDraft createdAsDraft);
        void Publish(Published published);
        void UpdateTitle(TitleUpdated titleUpdated);
        void UpdateText(TextUpdated textUpdated);
    }
}