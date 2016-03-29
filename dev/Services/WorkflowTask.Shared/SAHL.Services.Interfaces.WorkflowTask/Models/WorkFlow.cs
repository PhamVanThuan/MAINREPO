using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SAHL.Services.Interfaces.WorkflowTask
{
    [DataContract]
    public class WorkFlow
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public List<State> States { get; set; }
    }
}