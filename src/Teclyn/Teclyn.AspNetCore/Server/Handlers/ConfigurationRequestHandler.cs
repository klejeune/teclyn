using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Teclyn.Core.Api;

namespace Teclyn.AspNetCore.Server.Handlers
{
    public class ConfigurationRequestHandler : IRequestHandler
    {
        private readonly ITeclynApi _teclyn;
        private readonly AspNetCoreTranslater _translater;

        public ConfigurationRequestHandler(ITeclynApi teclyn, AspNetCoreTranslater translater)
        {
            this._teclyn = teclyn;
            this._translater = translater;
        }

        public string GetTemplate()
        {
            return this._teclyn.Configuration.CommandEndpointPrefix + "/.well-known/teclyn-configuration";
        }

        public RequestDelegate GetRequestDelegate()
        {
            return async context =>
            {
                var json = JsonConvert.SerializeObject(this._teclyn);

                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(json, context.RequestAborted);
            };
        }

        public string GetVerb()
        {
            return "GET";
        }
    }
}