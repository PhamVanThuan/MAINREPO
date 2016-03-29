using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SAHL.Services.Interfaces.WorkflowTask
{
    [DataContract]
    public class Task
    {
        [DataMember]
        public int InstanceId { get; set; }

        [DataMember]
        public List<string> Row { get; set; }

        [DataMember]
        public List<Guid> TagIds { get; set; }

        [DataMember]
        public string OriginationSource { get; set; }

        [DataMember]
        public string ProcessName { get; set; }

        [DataMember]
        public string WorkflowName { get; set; }

        [DataMember]
        public int GenericKey { get; set; }

        [DataMember]
        public int GenericKeyTypeKey { get; set; }

        [DataMember]
        public string Subject { get; set; }
    }
}