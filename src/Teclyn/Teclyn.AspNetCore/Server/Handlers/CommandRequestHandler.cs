using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using Teclyn.Core.Api;
using Teclyn.Core.Commands;

namespace Teclyn.AspNetCore.Server.Handlers
{
    public class CommandRequestHandler : IRequestHandler
    {
        private readonly ITeclynApi _teclyn;
        private readonly CommandService _commandService;
        private readonly AspNetCoreTranslater _translater;

        public CommandRequestHandler(ITeclynApi teclyn, CommandService commandService, AspNetCoreTranslater translater)
        {
            this._teclyn = teclyn;
            this._commandService = commandService;
            this._translater = translater;
        }

        public string GetTemplate()
        {
            return this._teclyn.Configuration.CommandEndpointPrefix + "/{domain}/{command}";
        }

        public RequestDelegate GetRequestDelegate()
        {
            return async context =>
            {
                var domainId = this._translater.ImportDomainId(context.GetRouteValue("domain").ToString());
                var commandId = this._translater.ImportCommandId(context.GetRouteValue("command").ToString());
                var commandInfo = this._teclyn.GetCommand(domainId, commandId);

                if (commandInfo != null)
                {
                    await this.ExecuteCommand(commandInfo, context);
                }
            };
        }

        public string GetVerb()
        {
            return "POST";
        }

        private async Task ExecuteCommand(CommandInfo commandInfo, HttpContext context)
        {
            var commandType = commandInfo.CommandType;
            var requestBody = this.GetRequestBody(context);

            var command = (ICommand)JsonConvert.DeserializeObject(requestBody, commandType);

            await this._commandService.Execute(command);
        }

        private string GetRequestBody(HttpContext context)
        {
            Stream req = context.Request.Body;
            string body = new StreamReader(req).ReadToEnd();

            return body;
        }
    }
}