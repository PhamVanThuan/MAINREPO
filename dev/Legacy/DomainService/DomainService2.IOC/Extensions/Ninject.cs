using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ninject;
using SAHL.X2.Common;

namespace DomainService2.IOC.Extensions
{
    /// <summary>
    /// Kernel Extension
    /// </summary>
    public static class KernelExtensions
    {
        /// <summary>
        /// Scans for Open Generic Types that Match the interface
        /// i.e. kernel.BindWithAllKnownConcreteTypes(Assembly.GetExecutingAssembly(), typeof(IDomainServiceCommandHandler<>)).DecorateWith(typeof(TransactionDecorator<>), typeof(LoggingDecorator<>));
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="kernel"></param>
        /// <param name="t"></param>
        public static AssemblyBinder BindWithAllKnownConcreteTypes(this IKernel kernel, Assembly assembly, Type contract)
        {
            var binder = new AssemblyBinder(kernel, assembly);
            binder.Bind(contract);
            return binder;
        }

        /// <summary>
        /// Decorate With
        /// </summary>
        /// <param name="kernel"></param>
        public static void DecorateWith(this AssemblyBinder assemblyBinder, params Type[] types)
        {
            assemblyBinder.DecorateWith(types);
        }
    }

    /// <summary>
    /// Assembly Binder
    /// </summary>
    public class AssemblyBinder
    {
        private IKernel kernel { get; set; }

        private Assembly assemblyToCheck { get; set; }

        private Type interfaceType;

        /// <summary>
        /// Initializes a new Assembly Binder
        /// </summary>
        /// <param name="kernel"></param>
        public AssemblyBinder(IKernel kernel, Assembly assembly)
        {
            this.kernel = kernel;
            this.assemblyToCheck = assembly;
            interfaceType = null;
        }

        /// <summary>
        /// Bind
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="kernel"></param>
        /// <param name="t"></param>
        public void Bind(Type interfaceType)
        {
            this.interfaceType = interfaceType;
        }

        public void BindConcreteTypes(Type interfaceType)
        {
            this.interfaceType = interfaceType;

            //Get all the concrete types that implement the interface
            var concreteTypesToRegister = GetConcreteTypesThatImplementInterface(assemblyToCheck, interfaceType);

            foreach (var concreteTypeToRegister in concreteTypesToRegister)
            {
                kernel.Bind(interfaceType).To(concreteTypeToRegister);
            }
        }

        public void BindConcreteTypesToWorkflowService(Type interfaceType)
        {
            this.interfaceType = interfaceType;

            //Get all the concrete types that implement the interface
            var concreteTypesToRegister = GetConcreteTypesThatImplementInterface(assemblyToCheck, interfaceType);

            foreach (var concreteTypeToRegister in concreteTypesToRegister)
            {
                Type[] interfaces = concreteTypeToRegister.GetInterfaces();

                Type typeToBindTo = interfaces.Where(x => x.GetInterface(typeof(IX2WorkflowService).FullName) != null).First();

                kernel.Bind(typeToBindTo).To(concreteTypeToRegister);
            }
        }

        /// <summary>
        /// Decorate the Type for the Binder with decorators
        /// </summary>
        /// <param name="decorators"></param>
        public void DecorateWith(params Type[] decorators)
        {
            //Get all the concrete types that implement the interface
            var handlersToRegister = GetConcreteTypesThatImplementInterface(assemblyToCheck, interfaceType);
            Type previousDecorator = null;

            string previousDecoratorName = String.Empty;
            string uniqueDecoratorName = String.Empty;
            string uniqueHandlerName = String.Empty;

            foreach (var handlerToRegister in handlersToRegister)
            {
                var genericInterfaceTypes = GetInterfacesForHandler(handlerToRegister, interfaceType);
                foreach (var genericInterfaceType in genericInterfaceTypes)
                {
                    var genericArgument = genericInterfaceType.GetGenericArguments().First();
                    var genericInterface = interfaceType.MakeGenericType(genericArgument);

                    previousDecorator = null;
                    foreach (var decorator in decorators)
                    {
                        var genericDecorator = decorator.MakeGenericType(genericArgument);

                        if (previousDecorator == null)
                        {
                            kernel.Bind(genericInterface).To(genericDecorator);
                            previousDecorator = genericDecorator;
                        }
                        else
                        {
                            kernel.Bind(genericInterface).To(genericDecorator).WhenInjectedInto(previousDecorator);
                            previousDecorator = genericDecorator;
                        }
                    }
                    kernel.Bind(genericInterface).To(handlerToRegister).WhenInjectedInto(previousDecorator);
                }
            }
        }

        /// <summary>
        /// Get Concrete Types That Implement Interface
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <returns></returns>
        private IList<Type> GetConcreteTypesThatImplementInterface(Assembly assembly, Type interfaceType)
        {
            return assembly.GetTypes().Where(x =>
                                !x.IsInterface &&
                                !x.IsGenericType &&
                                !x.IsAbstract &&
                                x.GetInterfaces().Where(i => i.Name == interfaceType.Name).Count() > 0
                              ).ToList();
        }

        /// <summary>
        /// Get Interfaces for Handler
        /// </summary>
        /// <param name="type"></param>
        /// <param name="handlerInterface"></param>
        /// <returns></returns>
        private IList<Type> GetInterfacesForHandler(Type type, Type handlerInterface)
        {
            return type.GetInterfaces().Where(x => x.Name == handlerInterface.Name).ToList();
        }
    }
}