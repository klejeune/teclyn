using System;

namespace Teclyn.Core.Domains
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DomainNameAttribute : Attribute
    {
        public string Name { get; }

        public DomainNameAttribute(string name)
        {
            this.Name = name;
        }
    }
}