using SAHL.Core.Services;
using System;

namespace SAHL.Core.Events
{
    public interface IEventSerialiser
    {
        string Serialise(IEvent eventToSerialise);

        T Deserialise<T>(string eventXmlData) where T : IEvent;

        string SerialiseEventMetadata(IServiceRequestMetadata metadata);

        IEvent Deserialise(Type eventType, string eventXmlData);

        IServiceRequestMetadata DeserialiseEventMetadata(string metadataXmlData);
    }
}