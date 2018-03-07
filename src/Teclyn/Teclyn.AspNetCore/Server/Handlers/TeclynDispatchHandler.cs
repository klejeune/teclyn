using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Teclyn.Core.Api;
using Teclyn.Core.Commands;
using Teclyn.Core.Dummies;
using Teclyn.Core.Queries;
using Teclyn.Core.Tools;

namespace Teclyn.AspNetCore.Server.Handlers
{
    public class TeclynDispatchHandler
    {
        private readonly ITeclynApi _teclyn;
        private readonly ConfigurationRequestHandler _configurationRequestHandler;
        private readonly CommandRequestHandler _commandRequestHandler;
        private readonly QueryRequestHandler _queryRequestHandler;
        private readonly CommandService _commandService;
        private readonly QueryService _queryService;
        private readonly AspNetCoreTranslater _translater;

        public TeclynDispatchHandler(ITeclynApi teclyn, ConfigurationRequestHandler configurationRequestHandler, CommandRequestHandler commandRequestHandler, QueryRequestHandler queryRequestHandler)
        {
            this._teclyn = teclyn;
            this._configurationRequestHandler = configurationRequestHandler;
            this._commandRequestHandler = commandRequestHandler;
            this._queryRequestHandler = queryRequestHandler;
        }

        public IRouter GetRouter(RouteBuilder builder)
        {
            var handlers = new IRequestHandler[]
            {
                this._configurationRequestHandler,
                this._commandRequestHandler,
                this._queryRequestHandler
            };

            foreach (var requestHandler in handlers)
            {
                builder.MapVerb(
                    requestHandler.GetVerb(),
                    requestHandler.GetTemplate(),
                    requestHandler.GetRequestDelegate());
            }

            return builder.Build();
        }
    }
}