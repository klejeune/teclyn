using System;

namespace Teclyn.Core.Ioc
{
    public class DependencyResolver : IDependencyResolver
    {
        private readonly IServiceProvider _serviceProvider;

        public DependencyResolver(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public T Get<T>()
        {
            return (T)this.Get(typeof(T));
        }

        public object Get(Type type)
        {
            return this._serviceProvider.GetService(type);
        }
    }
}