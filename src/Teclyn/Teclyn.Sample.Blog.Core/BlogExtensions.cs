using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Teclyn.Core.Api;
using Teclyn.Sample.Blog.Core.Posts;
using Teclyn.Sample.Blog.Core.Posts.Commands;
using Teclyn.Sample.Blog.Core.Posts.Events;
using Teclyn.Sample.Blog.Core.Posts.Models;
using Teclyn.Sample.Blog.Core.Posts.Queries;
using Teclyn.Sample.Blog.Core.Users;
using Teclyn.Sample.Blog.Core.Users.Commands;
using Teclyn.Sample.Blog.Core.Users.Events;
using Teclyn.Sample.Blog.Core.Users.Models;

namespace Teclyn.Sample.Blog.Core
{
    public static class BlogExtensions
    {
        public static void AddBlog(this IServiceCollection services, ITeclynApiConfiguration configuration)
        {
            services.AddTeclyn(new TeclynApi(configuration)
                .AddDomain<UserDomain>(d => d
                    .AddCommand<RegisterUserCommand, RegisterUserCommandHandler>()
                    .AddAggregate<IUser>(a => a
                        .AddEvent<Registered>()))
                .AddDomain<PostDomain>(d => d
                    .AddCommand<CreatePostAsDraft, CreatePostAsDraftCommandHandler>()
                    .AddCommand<PublishPost, PublishPostCommandHandler>()
                    .AddCommand<UpdatePostText, UpdatePostTextCommandHandler>()
                    .AddCommand<UpdatePostTitle, UpdatePostTitleCommandHandler>()
                    .AddQuery<ListPosts, ListPostsQueryHandler, IEnumerable<IPost>>()
                    .AddAggregate<IPost>(a => a
                        .AddEvent<CreatedAsDraft>()
                        .AddEvent<Published>()
                        .AddEvent<TextUpdated>()
                        .AddEvent<TitleUpdated>())));
        }
    }
}