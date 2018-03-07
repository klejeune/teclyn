using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Teclyn.Core.Commands;
using Teclyn.Core.Queries;
using Teclyn.Core.Security.Context;
using Teclyn.Core.Storage;
using Teclyn.Sample.Blog.Core.Posts.Models;

namespace Teclyn.Sample.Blog.Core.Posts.Queries
{
    public class ListPostsQueryHandler : IQueryHandler<ListPosts, IEnumerable<IPost>>
    {
        private readonly IRepository<IPost> _posts;

        public ListPostsQueryHandler(IRepository<IPost> posts)
        {
            this._posts = posts;
        }

        public Task<bool> CheckContext(ListPosts query, ITeclynContext context,IQueryContextChecker result)
        {
            return Task.FromResult(true);
        }

        public Task<bool> CheckParameters(ListPosts query, IParameterChecker result)
        {
            return Task.FromResult(true);
        }

        public Task<IEnumerable<IPost>> Execute(ListPosts query, IQueryExecutionContext<ListPosts, IEnumerable<IPost>> context)
        {
            context.Metadata.SetPagination(
                this._posts.LongCount(), 
                query.Page,
                query.PerPage, 
                page => new ListPosts { Page = page, PerPage = query.PerPage });
            
            return Task.FromResult(this._posts.OrderByDescending(p => p.PublicationDate)
                .Skip(query.PerPage * (query.Page - 1))
                .Take(query.Page).AsEnumerable());
        }
    }
}