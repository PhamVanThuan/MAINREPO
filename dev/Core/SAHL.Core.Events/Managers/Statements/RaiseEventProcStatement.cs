using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using System;

namespace SAHL.Core.Events.Managers.Statements
{
    public class RaiseEventProcStatement : ISqlStatement<string>
    {
        public RaiseEventProcStatement(DateTime effectiveDate, int genericKey, int genericKeyTypeKey, EventType eventTypeKey, string xmlEventData, string xmlMetaData)
        {
            this.EffectiveDate = effectiveDate;
            this.GenericKey = genericKey;
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.XmlEventData = xmlEventData;
            this.XmlMetaData = xmlMetaData;
            this.EventTypeKey = eventTypeKey;
        }

        public DateTime EffectiveDate { get; protected set; }

        public int GenericKey { get; protected set; }

        public int GenericKeyTypeKey { get; protected set; }

        public string XmlEventData { get; protected set; }

        public string XmlMetaData { get; protected set; }

        public EventType EventTypeKey { get; protected set; }

        public string ErrorMessage { get; set; }

        public string GetStatement()
        {
            return @"
                    declare @Msg varchar(1024) = NULL
                    -- Template table
                    select top 0 * into #GenericEvent from EventStore.[template].[GenericEvent]
                    insert into #GenericEvent ([GenericKey], [GenericKeyTypeKey], [EventTypeKey], [EventEffectiveDate]) values(@GenericKey, @GenericKeyTypeKey, @EventTypeKey, @EffectiveDate)
                    exec EventStore.[event].[RaiseEvent] @EventTypeKey, @GenericKey, @GenericKeyTypeKey, @EffectiveDate, '#GenericEvent', @XmlEventData, @XmlMetaData, @Msg OUTPUT
                    select @Msg";
        }
    }
}