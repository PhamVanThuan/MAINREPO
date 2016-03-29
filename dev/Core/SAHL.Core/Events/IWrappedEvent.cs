using SAHL.Core.Messaging.Shared;
using SAHL.Core.Services;

namespace SAHL.Core.Events
{
    public interface IWrappedEvent<out T> : IMessage where T : class , IEvent
    {
        T InternalEvent { get; }

        IServiceRequestMetadata ServiceRequestMetadata { get; }

        string Name { get; }
    }
}