using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ServiceLocation;
using Teclyn.Core.Ioc;

namespace Teclyn.AspNetMvc.Integration
{
    public class TeclynBasicServiceLocator : ServiceLocatorImplBase
    {
        [Inject]
        public IIocContainer IocContainer { get; set; }

        protected override object DoGetInstance(Type serviceType, string key)
        {
            return this.IocContainer.Get(serviceType);
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            return Enumerable.Empty<object>();
        }
    }
}