using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Teclyn.Core.Api;

namespace Teclyn.Core
{
    public class AttributeComputer
    {
        private IDictionary<IEnumerable<Type>, Action<IDictionary<Type, IEnumerable<TypeAttributeInfo>>>> handlers = new Dictionary<IEnumerable<Type>, Action<IDictionary<Type, IEnumerable<TypeAttributeInfo>>>>();
        
        public void RegisterHandler(IEnumerable<Type> attributes, Action<IDictionary<Type, IEnumerable<TypeAttributeInfo>>> action)
        {
            this.handlers[attributes] = action;
        }

        public void Compute(ITeclynConfiguration configuration)
        {
            var attributesToWatch = this.handlers.SelectMany(handler => handler.Key).Distinct();

            var types = configuration.Plugins
                .SelectMany(plugin => plugin.GetType().GetTypeInfo().Assembly.DefinedTypes)
                .SelectMany(
                    type => type
                        .GetCustomAttributes(true)
                        .Where(attribute => attributesToWatch.Contains(attribute.GetType()))
                        .Select(attribute => new TypeAttributeInfo { Type = type.AsType(), Attribute = attribute, AttributeType = attribute.GetType()}))
                .GroupBy(info => info.Attribute.GetType())
                .ToDictionary(group => group.Key, group => group.ToList().SafeCast<IEnumerable<TypeAttributeInfo>>());

            foreach (var handlerGroup in handlers)
            {
                var dictionary = handlerGroup.Key.ToDictionary(
                    attributeType => attributeType,
                    attributeType => types.GetValueOrDefault(attributeType, Enumerable.Empty<TypeAttributeInfo>()));

                handlerGroup.Value(dictionary);
            }
        }
    }
}