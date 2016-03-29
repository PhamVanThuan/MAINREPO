using SAHL.Core.Events.Projections.Wrappers;
using SAHL.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SAHL.Core.Events.Projections
{
    public class ProjectorReflectere : IProjectorReflectere
    {
        private readonly IEnumerable<Type> handleWrappers;
        private readonly IIocContainer container;

        public ProjectorReflectere(IIocContainer container)
        {
            this.container = container;
            this.handleWrappers = new[]{
                typeof(ServiceEventProjectionWrapper<,>),
                typeof(TableEventProjectionWrapper<,>),
            };
        }

        public IEnumerable<Type> GetInterfaces(IEventProjector projectorInstance)
        {
            return projectorInstance.GetType().GetImmediateInterfaces();
        }

        public Type GetEventType(Type projectionType)
        {
            IEnumerable<Type> genericArguments = projectionType.GetGenericArguments();
            return genericArguments.SingleOrDefault(x => typeof(IEvent).IsAssignableFrom(x));
        }

        public Delegate GetProjectionDelegate(IEventProjector projectorInstance, Type projectionType, Type eventType)
        {
            MethodInfo handleMethod = projectionType.GetMethods().First(x => x.Name == "Handle" && x.GetParameters().Any(parameter => parameter.ParameterType.Name == eventType.Name));
            Type genericAction = GetAction(handleMethod);
            Type projectionBaseType = projectionType.GetGenericTypeDefinition();

            if (projectionBaseType == null)
            {
                return null;
            }

            Type wrapperToUse = handleWrappers.SingleOrDefault(wrapper => (wrapper.GetInterfaces()[0]).GenericTypeArguments[1].Name == projectionBaseType.Name);

            if (wrapperToUse == null)
            {
                return null;
            }

            Delegate projectorHandleMethod = Delegate.CreateDelegate(genericAction, projectorInstance, handleMethod);

            Type genericEventWrapper = wrapperToUse.MakeGenericType(projectionType.GenericTypeArguments);
            var wrappedInstance = Activator.CreateInstance(genericEventWrapper, new object[] { container, projectorHandleMethod });
            var wrappedEventType = typeof(WrappedEvent<>).MakeGenericType(eventType);
            return Delegate.CreateDelegate(typeof(Action<>).MakeGenericType(wrappedEventType), wrappedInstance, wrappedInstance.GetType().GetMethods().First(method => method.Name == "Handle"));
        }

        private Type GetAction(MethodInfo methodInfo)
        {
            IEnumerable<ParameterInfo> parameters = methodInfo.GetParameters();
            Type[] types = parameters.Select(x => x.ParameterType).ToArray();
            switch (parameters.Count())
            {
                case 2:
                    return typeof(Action<,>).MakeGenericType(types);

                case 3:
                    return typeof(Action<,,>).MakeGenericType(types);

                default:
                    return null;
            }
        }
    }
}