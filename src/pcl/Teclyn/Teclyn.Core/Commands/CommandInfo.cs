using System;
using Teclyn.Core.Domains;

namespace Teclyn.Core.Commands
{
    public class CommandInfo : IDisplayable
    {
        public Type CommandType { get; }
        public string Name { get; }
        public string Id { get; }

        public CommandInfo(string id, string name, Type commandType)
        {
            this.Id = id;
            this.Name = name;
            this.CommandType = commandType;
        }
    }
}