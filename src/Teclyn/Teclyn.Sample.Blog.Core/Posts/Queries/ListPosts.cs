using System.Collections.Generic;
using Teclyn.Core.Queries;
using Teclyn.Sample.Blog.Core.Posts.Models;

namespace Teclyn.Sample.Blog.Core.Posts.Queries
{
    public class ListPosts : IQuery<IEnumerable<IPost>>
    {
        public int Page { get; set; }
        public int PerPage { get; set; }

        public ListPosts()
        {
            this.Page = 1;
            this.PerPage = 10;
        }
    }
}