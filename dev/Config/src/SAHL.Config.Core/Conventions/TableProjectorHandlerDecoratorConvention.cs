using System;
using System.Linq;
using System.Collections.Generic;

using StructureMap.Graph;
using StructureMap.Configuration.DSL;

using SAHL.Core.Data;
using SAHL.Core.Events;
using SAHL.Core.Extensions;
using SAHL.Config.Core.Decorators;
using SAHL.Core.Events.Projections;

namespace SAHL.Config.Core.Conventions
{
    public class TableProjectorHandlerDecoratorConvention : IRegistrationConvention
    {
        private IEnumerable<Type> decorators;

        public TableProjectorHandlerDecoratorConvention()
        {
            decorators = new[] { typeof(TableProjector<,>) };
        }

        public void Process(Type eventProjectorType, Registry registry)
        {
            if ((eventProjectorType.IsAssignableToGenericType(typeof(ITableProjector<,>))) &&
                eventProjectorType != typeof(TableProjector<,>) &&
                !eventProjectorType.IsAbstract)
            {
                var tableProjectorInterfaces = eventProjectorType.GetInterfaces().Where(x => x.IsAssignableToGenericType(typeof(ITableProjector<,>)));
                foreach (var tableProjectorInterface in tableProjectorInterfaces)
                {
                    var eventType = tableProjectorInterface.GetGenericArguments().First();
                    var dataModelType = tableProjectorInterface.GetGenericArguments().Last();
                    var enrichWithMethod = typeof(TableProjectorHandlerDecoratorConvention).GetMethod("EnrichWith");
                    var genericEnrichWithMethod = enrichWithMethod.MakeGenericMethod(new Type[] { eventType, dataModelType, eventProjectorType });
                    genericEnrichWithMethod.Invoke(null, new object[] { registry, decorators });
                }
            }
        }

        public static void EnrichWith<EventType, TDataModelType, EventProjectorType>(Registry registry, IEnumerable<Type> decorators)
            where EventType : IEvent
            where TDataModelType : IDataModel
            where EventProjectorType : ITableProjector<EventType, TDataModelType>
        {
            var registeredInstance = registry.For<IEventProjector<EventType>>().Use<EventProjectorType>().Named(typeof(EventProjectorType).Name);
            object previousDecorator = null;
            foreach (var decorator in decorators)
            {
                var genericDecoratorType = decorator.MakeGenericType(new[] { typeof(EventType), typeof(TDataModelType) });
                if (previousDecorator == null)
                {
                    var method = typeof(StructureMapDecorator).GetMethod("Decorate");
                    var genericMethod = method.MakeGenericMethod(typeof(EventProjectorType));
                    object decoratorHelper = genericMethod.Invoke(null, new object[] { registeredInstance });

                    var withMethod = decoratorHelper.GetType().GetMethod("With");
                    var genericWithMethod = withMethod.MakeGenericMethod(genericDecoratorType);
                    previousDecorator = genericWithMethod.Invoke(decoratorHelper, null);
                }
            }
        }
    }
}