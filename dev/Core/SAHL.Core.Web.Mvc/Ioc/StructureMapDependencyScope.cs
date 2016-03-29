using Microsoft.Practices.ServiceLocation;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;

namespace SAHL.Core.Web.Mvc.Ioc
{
    public class StructureMapDependencyScope : ServiceLocatorImplBase, IDependencyScope
    {
        public StructureMapDependencyScope(IContainer container)
        {
            if (container == null) { throw new ArgumentNullException("container"); }
            this.Container = container;
        }

        public void Dispose()
        {
            this.Container.Dispose();
        }

        protected IContainer Container { get; private set; }

        public override object GetService(Type serviceType)
        {
            if (serviceType == null) { return null; }

            System.Diagnostics.Debug.WriteLine(serviceType.FullName);
            return serviceType.IsAbstract || serviceType.IsInterface
                       ? this.Container.TryGetInstance(serviceType)
                       : this.Container.GetInstance(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this.Container.GetAllInstances(serviceType).Cast<object>();
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            return this.Container.GetAllInstances(serviceType).Cast<object>();
        }

        protected override object DoGetInstance(Type serviceType, string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return serviceType.IsAbstract || serviceType.IsInterface
                           ? this.Container.TryGetInstance(serviceType)
                           : this.Container.GetInstance(serviceType);
            }

            return this.Container.GetInstance(serviceType, key);
        }
    }
}