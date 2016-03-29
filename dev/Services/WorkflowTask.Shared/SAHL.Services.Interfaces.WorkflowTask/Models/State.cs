using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SAHL.Services.Interfaces.WorkflowTask
{
    [DataContract]
    public class State
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public List<string> ColumnHeaders { get; set; }

        [DataMember]
        public int TaskCount { get { return Tasks.Count;  } }

        [DataMember]
        public List<Task> Tasks { get; set; }
    }
}