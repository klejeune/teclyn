using System;

namespace Teclyn.Core.Api
{
    public class TypeAttributeInfo
    {
        public Type Type { get; set; }
        public Type AttributeType { get; set; }
        public Attribute Attribute { get; set; }
    }
}