using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Teclyn.Core.Api;
using Teclyn.Core.Dummies;
using Teclyn.Core.Queries;
using Teclyn.Core.Tools;

namespace Teclyn.AspNetCore.Server.Handlers
{
    public class QueryRequestHandler : IRequestHandler
    {
        private readonly ITeclynApi _teclyn;
        private readonly QueryService _queryService;
        private readonly AspNetCoreTranslater _translater;

        public QueryRequestHandler(ITeclynApi teclyn, QueryService queryService, AspNetCoreTranslater translater)
        {
            this._teclyn = teclyn;
            this._queryService = queryService;
            this._translater = translater;
        }

        public string GetTemplate()
        {
            return this._teclyn.Configuration.CommandEndpointPrefix + "/{domain}/{query}";
        }

        public RequestDelegate GetRequestDelegate()
        {
            return async context =>
            {
                var domainId = this._translater.ImportDomainId(context.GetRouteValue("domain").ToString());
                var queryId = this._translater.ImportQueryId(context.GetRouteValue("query").ToString());
                var domainInfo = this._teclyn.GetDomain(domainId);
                var queryInfo = this._teclyn.GetQuery(domainId, queryId);

                var method = ReflectionTools
                    .Instance<QueryRequestHandler>
                    .Method(_ => _.ExecuteQuery<DummyQuery, DummyQueryResult>(null, null, null))
                    .GetGenericMethodDefinition()
                    .MakeGenericMethod(queryInfo.QueryType, queryInfo.ResultType);

                await (Task)method.Invoke(this, new object[] { domainInfo, queryInfo, context });
            };
        }

        public string GetVerb()
        {
            return "GET";
        }

        private async Task ExecuteQuery<TQuery, TResult>(DomainInfo domainInfo, QueryInfo queryInfo, HttpContext context) where TQuery : IQuery<TResult>
        {
            if (queryInfo != null)
            {
                var result = await this.ExecuteQuery<TQuery, TResult>(context);
                var json = JsonConvert.SerializeObject(result.GetResult());

                var headers = this.GetMetadataLinkHeaders(domainInfo, queryInfo, context, result.Metadata);

                foreach (var header in headers)
                {
                    context.Response.Headers.Add(header.Key, header.Value);
                }

                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(json, context.RequestAborted);
            }
        }

        private IDictionary<string, StringValues> GetMetadataLinkHeaders<TQuery, TResult>(DomainInfo domainInfo, QueryInfo queryInfo, HttpContext context, QueryResultMetadata<TQuery, TResult> metadata)
            where TQuery : IQuery<TResult>
        {
            var headers = new Dictionary<string, StringValues>();

            if (metadata.Count.HasValue)
            {
                headers.Add("X-Total-Count", metadata.Count.ToString());
            }

            var links = new[]
                {
                    new {Name = "first", Value = metadata.First},
                    new {Name = "last", Value = metadata.Last},
                    new {Name = "prev", Value = metadata.Previous},
                    new {Name = "next", Value = metadata.Next},
                }
                .Where(_ => _.Value != null)
                .Select(_ => $"<{this.GetUrl<TQuery, TResult>(domainInfo, queryInfo, context, _.Value)}>; rel=\"{_.Name}\"")
                .ToArray();

            if (links.Any())
            {
                headers.Add("Link", new StringValues(links));
            }

            return headers;
        }

        private string GetUrl<TQuery, TResult>(DomainInfo domainInfo, QueryInfo queryInfo, HttpContext context, TQuery query) where TQuery : IQuery<TResult>
        {
            var baseUri = new Uri($"{context.Request.Scheme}://{context.Request.Host}/{context.Request.PathBase}");
            baseUri = new Uri(baseUri, this._translater.ExportDomainId(domainInfo) + "/");
            baseUri = new Uri(baseUri, this._translater.ExportQueryId(queryInfo) + "/");

            var url = baseUri + "?" + string.Join("&",
                          this.SerializeQuery<TQuery, TResult>(query).Select(_ => $"{_.Key}={_.Value}"));

            return url;
        }

        private IDictionary<string, string> SerializeQuery<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>
        {
            return typeof(TQuery)
                .GetProperties()
                .ToDictionary(p => p.Name, p => p.GetValue(query).ToString());
        }

        private async Task<IQueryResult<TQuery, TResult>> ExecuteQuery<TQuery, TResult>(HttpContext context) where TQuery : IQuery<TResult>
        {
            var query = this.ExtractQuery<TQuery, TResult>(context);

            return await this._queryService.Execute<TQuery, TResult>(query);
        }

        private TQuery ExtractQuery<TQuery, TResult>(HttpContext context)
        {
            var query = (TQuery)Activator.CreateInstance(typeof(TQuery));

            var properties = typeof(TQuery).GetProperties(
                BindingFlags.SetField | BindingFlags.GetField | BindingFlags.Instance | BindingFlags.Public);

            foreach (var property in properties)
            {
                var value = context.Request.Query[property.Name].FirstOrDefault();

                property.SetValue(query, value);
            }

            return query;
        }
    }
}