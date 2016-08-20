using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Teclyn.Core.Ioc
{
    public class BasicIocContainer : IIocContainer
    {
        private readonly IDictionary<Type, object> instances = new Dictionary<Type, object>();
        private readonly IDictionary<Type, Type> mappings = new Dictionary<Type, Type>();

        public void Initialize(IEnumerable<Assembly> assemblies)
        {
        }

        public T Get<T>()
        {
            return (T) this.Get(typeof(T));
        }

        public object Get(Type type)
        {
            object instance;

            if (!this.instances.TryGetValue(type, out instance))
            {
                instance = this.Build(type);
            }

            return instance;
        }

        private object Build(Type type)
        {
            Type concreteType;

            if (!this.mappings.TryGetValue(type, out concreteType))
            {
                concreteType = type;
            }

            var result = this.BuildConcrete(concreteType);
            this.Inject(result);

            return result;
        }

        private object BuildConcrete(Type concreteType)
        {
            var constructors = concreteType.GetTypeInfo().DeclaredConstructors.Where(c => !c.IsStatic);

            if (constructors.Count() > 1)
            {
                throw new TeclynException($"Unable to build type {concreteType}: it has more than one constructor.");
            }

            var constructor = constructors.Single();

            var parameters = constructor
                .GetParameters()
                .Select(parameter => parameter.ParameterType)
                .Select(this.Get).ToArray();

            var builtObject = constructor.Invoke(parameters);

            this.Inject(builtObject);

            return builtObject;
        }

        public void Register<TPublicType, TImplementation>() where TImplementation : TPublicType
        {
            this.Register(typeof(TPublicType), typeof(TImplementation));
        }

        public void Register<TPublicType>() where TPublicType : class
        {
            this.Register<TPublicType, TPublicType>();
        }

        public void RegisterSingleton<TPublicType>() where TPublicType : class
        {
            this.Register<TPublicType, TPublicType>();
        }

        public void Register<TPublicType>(TPublicType @object)
        {
            this.instances[typeof(TPublicType)] = @object;
        }

        public void Register(Type publicType, Type implementationType)
        {
            this.mappings[publicType] = implementationType;
        }

        public void Inject(object item)
        {
            foreach (var property in this.GetAllProperties(item.GetType()).Where(this.MustInject))
            {
                this.InjectProperty(property, item);
            }
        }

        private void InjectProperty(PropertyInfo property, object container)
        {
            var setFunction = property.SetMethod;

            if (setFunction == null)
            {
                setFunction = property.SetMethod;
            }

            if (setFunction == null)
            {
                throw new TeclynException($"The property {property.DeclaringType} of type {property.Name} can't be injected: it doesn't have a mutator.");
            }

            //try
            //{
                var value = this.Get(property.PropertyType);

                setFunction.Invoke(container, new[] { value });
            //}
            //catch (TypeBuildException exception)
            //{
            //    throw new TechnicalException(string.Format("Unable to inject {0}.{1} of type {2}: no implementation was found.", property.DeclaringType.Name, property.Name, property.PropertyType.Name));
            //}
        }

        private IEnumerable<PropertyInfo> GetAllProperties(Type type)
        {
            return type.GetRuntimeProperties();
        }

        private bool MustInject(PropertyInfo property)
        {
            return property.GetCustomAttribute<InjectAttribute>() != null;
        }
    }
}