using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.EventStore
{
    [Serializable]
    public partial class EventTypeDataModel :  IDataModel
    {
        public EventTypeDataModel(int eventTypeKey, string name, string className, int? domainKey, int version)
        {
            this.EventTypeKey = eventTypeKey;
            this.Name = name;
            this.ClassName = className;
            this.DomainKey = domainKey;
            this.Version = version;
		
        }		

        public int EventTypeKey { get; set; }

        public string Name { get; set; }

        public string ClassName { get; set; }

        public int? DomainKey { get; set; }

        public int Version { get; set; }
    }
}