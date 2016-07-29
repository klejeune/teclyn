using System;
using System.Reflection;

namespace Teclyn.Core.Events.Injection
{
    public class AttributeEventInjector<TAttribute, TData> : IEventInjector where TAttribute : Attribute
    {
        private readonly Func<TData> dataBuilder;

        public AttributeEventInjector(Func<TData> dataBuilder)
        {
            this.dataBuilder = dataBuilder;
        }

        public bool AppliesToProperty(PropertyInfo property)
        {
            return property.GetCustomAttribute<TAttribute>() != null;
        }

        public void Inject(ITeclynEvent @event, PropertyInfo property)
        {
            property.SetValue(@event, this.dataBuilder());
        }
    }
}