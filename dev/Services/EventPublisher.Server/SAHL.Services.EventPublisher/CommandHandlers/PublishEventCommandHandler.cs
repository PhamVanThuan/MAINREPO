using SAHL.Core.Events;
using SAHL.Core.Messaging;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.EventPublisher.Services;
using SAHL.Services.Interfaces.EventPublisher.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace SAHL.Services.EventPublisher.CommandHandlers
{
    public class PublishEventCommandHandler : IServiceCommandHandler<PublishEventCommand>
    {
        private readonly IEventPublisherDataService _eventPublisherDataService;
        private readonly IEventSerialiser _eventSerialiser;
        private readonly IMessageBusAdvanced _messageBus;

        public PublishEventCommandHandler(IEventPublisherDataService eventPublisherDataService, IEventSerialiser eventSerialiser, IMessageBusAdvanced messageBus)
        {
            this._eventPublisherDataService = eventPublisherDataService;
            this._eventSerialiser = eventSerialiser;
            this._messageBus = messageBus;
        }

        public ISystemMessageCollection HandleCommand(PublishEventCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();

            var eventDataModel = _eventPublisherDataService.GetEventDataModelByEventKey(command.EventKey);
            if (eventDataModel == null || string.IsNullOrEmpty(eventDataModel.Data))
            {
                messages.AddMessage(new SystemMessage("Could not get event data", SystemMessageSeverityEnum.Error));
            }
            else
            {
                var eventToPublish = this.LoadEvent(eventDataModel.Data, messages);
                if (eventToPublish == null) { return messages; }
                var eventMetaData = this.LoadMetaData(eventDataModel.Metadata, messages);
                if (eventMetaData == null) { return messages; }
                var wrappedEvent = this.CreateWrappedEvent(eventToPublish, eventMetaData);
                _messageBus.Publish(wrappedEvent);
            }
            return messages;
        }

        private IServiceRequestMetadata LoadMetaData(string metaData, ISystemMessageCollection messages)
        {
            IServiceRequestMetadata metadata = null;
            try
            {
                metadata = _eventSerialiser.DeserialiseEventMetadata(metaData);
                if (metadata == null)
                {
                    throw new Exception("Could not create metaData to publish");
                }
            }
            catch (Exception runtimeException)
            {
                var exceptionMessage = new SystemMessage(runtimeException.Message, SystemMessageSeverityEnum.Error);
                messages.AddMessage(exceptionMessage);

                if (runtimeException.InnerException != null)
                {
                    var innerExceptionMessage = new SystemMessage(runtimeException.InnerException.Message, SystemMessageSeverityEnum.Error);
                    messages.AddMessage(innerExceptionMessage);
                }
            }
            return metadata;
        }

        private IEvent LoadEvent(string eventData, ISystemMessageCollection messages)
        {
            IEvent eventToPublish = null;
            try
            {
                var eventType = this.GetEventClassType(eventData);
                eventToPublish = _eventSerialiser.Deserialise(eventType, eventData);
                if (eventToPublish == null)
                {
                    throw new Exception("Could not create event to publish");
                }
            }
            catch (Exception runtimeException)
            {
                var exceptionMessage = new SystemMessage(runtimeException.Message, SystemMessageSeverityEnum.Error);
                messages.AddMessage(exceptionMessage);

                if (runtimeException.InnerException != null)
                {
                    var innerExceptionMessage = new SystemMessage(runtimeException.InnerException.Message, SystemMessageSeverityEnum.Error);
                    messages.AddMessage(innerExceptionMessage);
                }
            }
            return eventToPublish;
        }

        private Type GetEventClassType(string data)
        {
            var xDocument = XDocument.Parse(data);
            var className = xDocument.Descendants().Elements("ClassName").SingleOrDefault();

            if ((className == null) || string.IsNullOrWhiteSpace(className.Value))
            {
                throw new Exception("Event xml does not contain a ClassName element");
            }

            var eventType = Type.GetType(className.Value);
            if (eventType == null)
            {
                throw new Exception(string.Format("Unknown event type: {0}", className));
            }

            return eventType;
        }

        private IWrappedEvent<T> CreateWrappedEvent<T>(T eventToPublish, IServiceRequestMetadata metadata) where T : class, IEvent
        {
            var baseWrappedEvent = typeof(WrappedEvent<>);
            var eventType = eventToPublish.GetType();
            var genericType = baseWrappedEvent.MakeGenericType(eventType);

            var constructorInfo = genericType.GetConstructors().OrderBy(info => info.GetParameters().Length).FirstOrDefault();
            if (constructorInfo == null)
            {
                throw new Exception("Unable to create the Wrapped event type");
            }

            var constructorParams = new Dictionary<string, object>
                {
                    { "id", eventToPublish.Id },
                    { "internalEvent", eventToPublish },
                    { "serviceRequestMetadata", metadata }
                };

            var wrappedEvent = (IWrappedEvent<T>)constructorInfo.Invoke(constructorParams.Select(x => x.Value).ToArray());
            return wrappedEvent;
        }
    }
}