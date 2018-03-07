using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;
using Teclyn.AspNetCore.Server.Handlers;
using Teclyn.AspNetCore.Swagger;
using Teclyn.Core.Api;
using Teclyn.Core.Commands;
using Teclyn.Core.Ioc;
using Teclyn.Core.Queries;

namespace Teclyn.AspNetCore
{
    public static class StartupExtensions
    {
        public static void AddTeclynAspNetCore(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.AddApplicationPart(typeof(StartupExtensions).Assembly);
            mvcBuilder.Services.AddTransient<TeclynExecutionHandler>();
            mvcBuilder.Services.AddTransient<AspNetCoreTranslater>();
        }

        public static void UseTeclyn(this IApplicationBuilder app)
        {
            var router = app.ApplicationServices
                .GetService<TeclynExecutionHandler>()
                .GetRouter(new RouteBuilder(app));
            
            app.UseRouter(router);
        }

        public static void UseTeclyn(this SwaggerGenOptions swaggerGenOptions)
        {
            swaggerGenOptions.CustomSchemaIds(type =>
            {
                var friendlyType = type.FriendlyId(false);

                if (friendlyType.Length >= 3 && friendlyType.StartsWith("I") && char.IsUpper(friendlyType[1]))
                {
                    return friendlyType.Substring(1);
                }
                else
                {
                    return friendlyType;
                }
            });

            swaggerGenOptions.DocumentFilter<TeclynDocumentFilter>();
        }
    }
}