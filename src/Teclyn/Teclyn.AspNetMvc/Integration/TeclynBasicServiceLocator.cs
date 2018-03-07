using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ServiceLocation;
using Teclyn.Core.Ioc;

namespace Teclyn.AspNetMvc.Integration
{
    public class TeclynBasicServiceLocator : ServiceLocatorImplBase
    {
        public IDependencyResolver DependencyResolver { get; set; }

        protected override object DoGetInstance(Type serviceType, string key)
        {
            return this.DependencyResolver.Get(serviceType);
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            return Enumerable.Empty<object>();
        }
    }
}