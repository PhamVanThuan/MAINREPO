using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.EventStore
{
    [Serializable]
    public partial class EventDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public EventDataModel(int eventTypeKey, int genericKey, int genericKeyTypeKey, string data, string correlationID, DateTime eventInsertDate, DateTime eventEffectiveDate, bool? processed, string metadata)
        {
            this.EventTypeKey = eventTypeKey;
            this.GenericKey = genericKey;
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.Data = data;
            this.CorrelationID = correlationID;
            this.EventInsertDate = eventInsertDate;
            this.EventEffectiveDate = eventEffectiveDate;
            this.Processed = processed;
            this.Metadata = metadata;
		
        }
		[JsonConstructor]
        public EventDataModel(int eventKey, int eventTypeKey, int genericKey, int genericKeyTypeKey, string data, string correlationID, DateTime eventInsertDate, DateTime eventEffectiveDate, bool? processed, string metadata)
        {
            this.EventKey = eventKey;
            this.EventTypeKey = eventTypeKey;
            this.GenericKey = genericKey;
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.Data = data;
            this.CorrelationID = correlationID;
            this.EventInsertDate = eventInsertDate;
            this.EventEffectiveDate = eventEffectiveDate;
            this.Processed = processed;
            this.Metadata = metadata;
		
        }		

        public int EventKey { get; set; }

        public int EventTypeKey { get; set; }

        public int GenericKey { get; set; }

        public int GenericKeyTypeKey { get; set; }

        public string Data { get; set; }

        public string CorrelationID { get; set; }

        public DateTime EventInsertDate { get; set; }

        public DateTime EventEffectiveDate { get; set; }

        public bool? Processed { get; set; }

        public string Metadata { get; set; }

        public void SetKey(int key)
        {
            this.EventKey =  key;
        }
    }
}