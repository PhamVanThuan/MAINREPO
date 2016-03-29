using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SAHL.Services.Interfaces.WorkflowTask
{
    [DataContract]
    public class BusinessProcess
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public List<WorkFlow> WorkFlows { get; set; }
    }
}