using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Teclyn.Core.Api
{
    public class QueryInfo
    {
        public QueryInfo(string id, string name, Type commandType, Type commandHandlerType, Type commandHandlerImplementationType, Type resultType, IDictionary<string, Type> parameters)
        {
            this.Id = id;
            this.Name = name;
            this.QueryType = commandType;
            this.QueryHandlerType = commandHandlerType;
            this.QueryHandlerImplementationType = commandHandlerImplementationType;
            this.ResultType = resultType;
            this.Parameters = new ReadOnlyDictionary<string, Type>(parameters);
        }

        public string Id { get; }
        public string Name { get; }
        public Type QueryType { get; }
        public Type QueryHandlerType { get; }
        public Type QueryHandlerImplementationType { get; }
        public Type ResultType { get; }
        public IReadOnlyDictionary<string, Type> Parameters { get; }
    }
}