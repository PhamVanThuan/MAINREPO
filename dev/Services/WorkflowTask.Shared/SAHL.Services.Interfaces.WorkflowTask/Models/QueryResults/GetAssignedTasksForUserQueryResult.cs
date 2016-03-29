using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.WorkflowTask
{
    [DataContract]
    public class GetAssignedTasksForUserQueryResult
    {
        [DataMember]
        public List<BusinessProcess> BusinessProcesses { get; set; }
    }
}

