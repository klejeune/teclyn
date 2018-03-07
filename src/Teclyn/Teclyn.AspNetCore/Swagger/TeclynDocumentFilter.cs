using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Teclyn.Core.Api;

namespace Teclyn.AspNetCore.Swagger
{
    public class TeclynDocumentFilter : IDocumentFilter
    {
        private readonly ITeclynApi _teclyn;
        private readonly AspNetCoreTranslater _translater;

        public TeclynDocumentFilter(ITeclynApi teclyn, AspNetCoreTranslater translater)
        {
            this._teclyn = teclyn;
            this._translater = translater;
        }

        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            this.AddConfiguration(swaggerDoc, context);

            foreach (var domain in this._teclyn.Domains)
            {
                this.AddDomain(swaggerDoc, context, domain);
            }
        }

        private void AddDomain(SwaggerDocument swaggerDoc, DocumentFilterContext context, DomainInfo domain)
        {
            foreach (var command in domain.Commands)
            {
                this.AddCommand(swaggerDoc, context, domain, command);
            }

            foreach (var query in domain.Queries)
            {
                this.AddQuery(swaggerDoc, context, domain, query);
            }
        }

        private void AddCommand(SwaggerDocument swaggerDoc, DocumentFilterContext context, DomainInfo domain, CommandInfo command)
        {
            swaggerDoc.Paths.Add($"/{this._teclyn.Configuration.CommandEndpointPrefix}/{this._translater.ExportDomainId(domain)}/{this._translater.ExportCommandId(command)}", new PathItem
            {
                Post = new Operation
                {
                    Tags = new List<string> {domain.Name},
                    Consumes = new List<string>
                    {
                        "application/json",
                    },
                    Parameters = new IParameter[]
                    {
                        new BodyParameter
                        {
                            Name = "command",
                            Schema = context.SchemaRegistry.GetOrRegister(command.CommandType),
                            In = "body",
                        }
                    },
                    Responses = new Dictionary<string, Response>
                    {
                        { "200", new Response{Description = "Success"} }
                    }
                }
            });
        }

        private void AddQuery(SwaggerDocument swaggerDoc, DocumentFilterContext context, DomainInfo domain, QueryInfo query)
        {
            swaggerDoc.Paths.Add($"/{this._teclyn.Configuration.CommandEndpointPrefix}/{this._translater.ExportDomainId(domain)}/{this._translater.ExportQueryId(query)}", new PathItem
            {
                Get = new Operation
                {
                    Tags = new List<string> { domain.Name },
                    Consumes = new List<string>
                    {
                        "application/json",
                    },
                    Parameters = query.Parameters.Select(p => this.BuildParameter(p.Key, p.Value)).ToList(),
                    Responses = new Dictionary<string, Response>
                    {
                        { "200", new Response{Description = "Success", Schema = this.GetSchema(context, query.ResultType)} }
                    }
                }
            });
        }

        private void AddConfiguration(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Paths.Add($"/{this._teclyn.Configuration.CommandEndpointPrefix}/.well-known/teclyn-configuration", new PathItem
            {
                Get = new Operation()
                {
                    Tags = "Configuration".AsArray().ToList(),
                    Consumes = new List<string>
                    {
                        "application/json",
                    },
                    Responses = new Dictionary<string, Response>
                    {
                        { "200", new Response{Description = "Success", Schema = this.GetSchema(context, typeof(ITeclynApi))} }
                    }
                }
            });
        }

        private Schema GetSchema(DocumentFilterContext context, Type type)
        {
            var schema = context.SchemaRegistry.GetOrRegister(type);

            schema = this.CleanSchema(schema);

            return schema;
        }

        private IParameter BuildParameter(string name, Type type)
        {
            var schemaBuilder = PrimitiveTypeMap.GetValueOrDefault(type);

            if (schemaBuilder == null)
            {
                throw new InvalidOperationException($"The {type.Name} is not a supported query parameter type.");
            }

            var schema = schemaBuilder();

            return new NonBodyParameter
            {
                Name = name,
                In = "query",
                Type = schema.Type,
                Format = schema.Format,
            };
        }

        private static readonly Dictionary<Type, Func<Schema>> PrimitiveTypeMap = new Dictionary<Type, Func<Schema>>
        {
            { typeof(short), () => new Schema { Type = "integer", Format = "int32" } },
            { typeof(ushort), () => new Schema { Type = "integer", Format = "int32" } },
            { typeof(int), () => new Schema { Type = "integer", Format = "int32" } },
            { typeof(uint), () => new Schema { Type = "integer", Format = "int32" } },
            { typeof(long), () => new Schema { Type = "integer", Format = "int64" } },
            { typeof(ulong), () => new Schema { Type = "integer", Format = "int64" } },
            { typeof(float), () => new Schema { Type = "number", Format = "float" } },
            { typeof(double), () => new Schema { Type = "number", Format = "double" } },
            { typeof(decimal), () => new Schema { Type = "number", Format = "double" } },
            { typeof(byte), () => new Schema { Type = "string", Format = "byte" } },
            { typeof(sbyte), () => new Schema { Type = "string", Format = "byte" } },
            { typeof(byte[]), () => new Schema { Type = "string", Format = "byte" } },
            { typeof(sbyte[]), () => new Schema { Type = "string", Format = "byte" } },
            { typeof(bool), () => new Schema { Type = "boolean" } },
            { typeof(DateTime), () => new Schema { Type = "string", Format = "date-time" } },
            { typeof(DateTimeOffset), () => new Schema { Type = "string", Format = "date-time" } },
            { typeof(Guid), () => new Schema { Type = "string", Format = "uuid" } }
        };

        private Schema CleanSchema(Schema schema)
        {
            schema.ReadOnly = null;

            if (schema.Properties != null)
            {
                foreach (var field in schema.Properties)
                {
                    this.CleanSchema(field.Value);
                }
            }

            return schema;
        }
    }
}