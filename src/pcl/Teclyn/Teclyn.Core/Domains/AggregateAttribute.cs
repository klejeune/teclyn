using System;

namespace Teclyn.Core.Domains
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
    public class AggregateAttribute : Attribute
    {
        public Type DefaultFilter { get; set; }
        public Type AccessController { get; set; }
    }
}