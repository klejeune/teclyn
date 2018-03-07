using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Teclyn.Core.Events.Injection
{
    public class EventInjector
    {
        private IList<IEventInjector> injectors = new List<IEventInjector>();

        public void Inject(ITeclynEvent @event)
        {
            var properties = @event.GetType().GetRuntimeProperties().Where(property => property.CanWrite);

            var injectionActions = properties
                .Select(property => new
                {
                    Injector = injectors.FirstOrDefault(injector => injector.AppliesToProperty(property)),
                    Property = property
                })
                .Where(info => info.Injector != null)
                .Select(info => (Action) (() => info.Injector.Inject(@event, info.Property)));

            foreach (var injectionAction in injectionActions)
            {
                injectionAction();
            }
        }

        public void RegisterAttribute<TAttribute, TData>(Func<TData> injector) where TAttribute : Attribute
        {
            this.injectors.Add(new AttributeEventInjector<TAttribute, TData>(injector));
        }
    }
}