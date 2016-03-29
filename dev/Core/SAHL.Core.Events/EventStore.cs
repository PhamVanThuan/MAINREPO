using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Events.Managers.Statements;
using SAHL.Core.Services;
using System;
using System.Linq;

namespace SAHL.Core.Events
{
    internal class EventStore : IEventStore
    {
        private IEventSerialiser eventSerialiser;
        private IDbFactory dbFactory;
        private string metadataXmlData;

        internal EventStore(IEventSerialiser eventSerialiser, IDbFactory dbFactory)
        {
            this.eventSerialiser = eventSerialiser;
            this.dbFactory = dbFactory;
            this.metadataXmlData = string.Empty;
        }

        void IEventStore.StoreEvent(DateTime eventOccuranceDate, IEvent eventToStore, int genericKey, int genericKeyTypeKey, IServiceRequestMetadata metadata)
        {
            // convert the event to an xml packet for the event store
            string eventXmlData = this.eventSerialiser.Serialise(eventToStore);
            metadataXmlData = this.eventSerialiser.SerialiseEventMetadata(metadata);

            EventType eventType = EventType.Domain_Event;
            if (eventToStore.GetType().GetInterfaces().Any(x => x.Name.Contains("ILegacyEvent")))
            {
                eventType = EventType.Legacy_Event;
            }

            if (eventToStore.GetType().GetInterfaces().Any(x => x.Name.Contains("IWorkflowEvent")))
            {
                eventType = EventType.Workflow_Event;
            }

            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                var raiseEventStatement = new RaiseEventProcStatement(eventToStore.Date, genericKey, genericKeyTypeKey, eventType, eventXmlData, metadataXmlData);
                var errors = db.SelectOne<string>(raiseEventStatement);
                if (!string.IsNullOrEmpty(errors))
                {
                    throw new Exception(errors);
                }
                db.Complete();
            }
        }
    }
}