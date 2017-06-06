using System;
using System.Reflection;
using Teclyn.Core.Domains;

namespace Teclyn.Core.Commands
{
    public abstract class Id
    {
        public string Value { get; }

        protected Id(string stringId)
        {
            this.Value = stringId;
        }

        public static Id<TAggregate> From<TAggregate>(string stringId) where TAggregate : IAggregate
        {
            return new Id<TAggregate>(stringId);
        }

        public static Id From(Type aggregateType, string stringId) 
        {
            if (!typeof(IAggregate).GetTypeInfo().IsAssignableFrom(aggregateType.GetTypeInfo()))
            {
                throw new TeclynException($"Could not create ID: the type {aggregateType} should implement {typeof(IAggregate).Name}");
            }

            var type = typeof(Id<>).MakeGenericType(aggregateType);
            return (Id) Activator.CreateInstance(type, stringId);
        }
    }

    public class Id<TAggregate> : Id where TAggregate : IAggregate
    {
        public Id(string stringId) : base(stringId)
        {
        }

        public static implicit operator Id<TAggregate>(string stringId)
        {
            return new Id<TAggregate>(stringId);
        }
    }
}