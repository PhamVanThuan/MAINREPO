using System;
using System.Reflection;
using DomainService2;
using Ninject;

namespace SAHLDomainService.Extensions
{
    public static class DomainServiceExtensions
    {
        public static AssemblyBinder BindAllRemotingHosts(this IKernel kernel)
        {
            var binder = new AssemblyBinder(kernel, Assembly.GetAssembly(typeof(IRemotingService)));
            binder.BindConcreteTypes(typeof(IRemotingService));
            return binder;
        }

        public static AssemblyBinder BindAllCommandHandlersAndDecorateWith(this IKernel kernel, params Type[] types)
        {
            var binder = new AssemblyBinder(kernel, Assembly.GetAssembly(typeof(IDomainServiceCommand)));
            binder.Bind(typeof(IHandlesDomainServiceCommand<>));
            binder.DecorateWith(types);
            return binder;
        }

        //kernel.BindWithAllKnownConcreteTypes(Assembly.GetExecutingAssembly(), typeof(IDomainServiceCommandHandler<>)).DecorateWith(typeof(TransactionDecorator<>), typeof(LoggingDecorator<>));
    }
}