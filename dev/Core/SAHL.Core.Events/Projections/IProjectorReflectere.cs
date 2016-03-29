using System;
using System.Collections.Generic;

namespace SAHL.Core.Events.Projections
{
    public interface IProjectorReflectere
    {
        IEnumerable<Type> GetInterfaces(IEventProjector projectorInstance);

        Type GetEventType(Type projectionType);

        Delegate GetProjectionDelegate(IEventProjector projectorInstance, Type projectionType, Type eventType);
    }
}