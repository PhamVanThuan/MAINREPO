using System;
using System.Reflection;
using Ninject;

namespace DomainService2.IOC.Extensions
{
    public static class DomainServiceExtensions
    {
        public static AssemblyBinder BindAllDomainHosts(this IKernel kernel)
        {
            var binder = new AssemblyBinder(kernel, Assembly.GetAssembly(typeof(IDomainHost)));
            binder.BindConcreteTypesToWorkflowService(typeof(IDomainHost));
            return binder;
        }

        public static AssemblyBinder BindAllCommandHandlersAndDecorateWith(this IKernel kernel, params Type[] types)
        {
            var binder = new AssemblyBinder(kernel, Assembly.GetAssembly(typeof(IDomainServiceCommand)));
            binder.Bind(typeof(IHandlesDomainServiceCommand<>));
            binder.DecorateWith(types);
            return binder;
        }
    }
}