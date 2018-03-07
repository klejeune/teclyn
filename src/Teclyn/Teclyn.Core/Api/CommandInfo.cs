using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Teclyn.Core.Api
{
    public class CommandInfo
    {
        public CommandInfo(string id, string name, Type commandType, Type commandHandlerType, Type commandHandlerImplementationType, IEnumerable<CommandParameterInfo> parameters)
        {
            this.Id = id;
            this.CommandType = commandType;
            this.CommandHandlerType = commandHandlerType;
            this.CommandHandlerImplementationType = commandHandlerImplementationType;
            this.Parameters = parameters;
            this.Name = name;
        }

        public string Name { get; }
        public string Id { get; }
        public Type CommandType { get; }
        public Type CommandHandlerType { get; }
        public Type CommandHandlerImplementationType { get; }
        public IEnumerable<CommandParameterInfo> Parameters { get; }
    }
}