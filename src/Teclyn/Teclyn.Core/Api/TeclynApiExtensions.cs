using Microsoft.Extensions.DependencyInjection;
using Teclyn.Core.Commands;
using Teclyn.Core.Events;
using Teclyn.Core.Ioc;
using Teclyn.Core.Queries;
using Teclyn.Core.Security.Context;
using Teclyn.Core.Services;
using Teclyn.Core.Tools;

namespace Teclyn.Core.Api
{
    public static class TeclynApiExtensions
    {
        public static void AddTeclyn(this IServiceCollection services, ITeclynApi teclyn)
        {
            services.AddServices();
            services.AddSingleton(teclyn);

            foreach (var domain in teclyn.Domains)
            {
                foreach (var command in domain.Commands)
                {
                    services.AddTransient(command.CommandHandlerType, command.CommandHandlerImplementationType);
                }

                foreach (var query in domain.Queries)
                {
                    services.AddTransient(query.QueryHandlerType, query.QueryHandlerImplementationType);
                }

                foreach (var aggregate in domain.Aggregates)
                {
                    services.AddTransient(aggregate.RepositoryType, aggregate.RepositoryImplementationType);
                    services.AddTransient(aggregate.RepositoryProviderType, aggregate.RepositoryProviderImplementationType);
                }
            }
        }

        private static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<ITimeService, TimeService>();
            services.AddTransient<IdGenerator>();
            services.AddTransient<CommandService>();
            services.AddTransient<QueryService>();
            services.AddTransient<ITeclynContext, TeclynContext>();
            services.AddTransient<IEventService, EventService>();
            services.AddTransient<IDependencyResolver, DependencyResolver>();
        }
    }
}