using System;
using System.Collections;
using System.Collections.Concurrent;
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
        private IDictionary<IEnumerable<Predicate<Type>>, Action<IDictionary<Predicate<Type>, IEnumerable<Type>>>> predicates = new Dictionary<IEnumerable<Predicate<Type>>, Action<IDictionary<Predicate<Type>, IEnumerable<Type>>>>();

        //public void RegisterHandler(IEnumerable<Type> attributes, Action<IDictionary<Type, IEnumerable<TypeAttributeInfo>>> action)
        //{
        //    this.handlers[attributes] = action;

            
        //}

        public void RegisterHandler(IEnumerable<Predicate<Type>> predicateGroup, Action<IDictionary<Predicate<Type>, IEnumerable<Type>>> action)
        {
            this.predicates[predicateGroup] = action;
        }

        public void Compute(IEnumerable<Assembly> assemblies)
        {
            var predicatesToWatch = this.predicates.SelectMany(predicate => predicate.Key).Distinct();

            var types = assemblies
                .SelectMany(assembly => assembly.DefinedTypes)
                .SelectMany(
                    type => predicatesToWatch
                        .Select(p => new { Type = type.AsType(), Predicate = p })
                        .Where(predicateInfo => predicateInfo.Predicate(predicateInfo.Type)))
                .GroupBy(predicateInfo => predicateInfo.Predicate)
                .ToDictionary(predicateGroup => predicateGroup.Key, predicateGroup => predicateGroup.Select(predicateInfo => predicateInfo.Type).ToList().SafeCast<IEnumerable<Type>>());


                    //type
                    //    .GetCustomAttributes(true)
                    //    .Where(attribute => attributesToWatch.Contains(attribute.GetType()))
                    //    .Select(attribute => new TypeAttributeInfo { Type = type.AsType(), Attribute = attribute, AttributeType = attribute.GetType() }))
                //.GroupBy(info => info.Predicate.GetType())
                //.ToDictionary(group => group.Key, group => group.ToList().SafeCast<IEnumerable<TypeAttributeInfo>>());

            foreach (var predicateGroup in predicates)
            {
                var dictionary = predicateGroup.Key.ToDictionary(
                    attributeType => attributeType,
                    attributeType => types.GetValueOrDefault(attributeType, Enumerable.Empty<Type>()));

                predicateGroup.Value(dictionary);
            }
        }

        public void ComputeOld(IEnumerable<Assembly> assemblies)
        {
            var attributesToWatch = this.handlers.SelectMany(handler => handler.Key).Distinct();

            var types = assemblies
                .SelectMany(assembly => assembly.DefinedTypes)
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