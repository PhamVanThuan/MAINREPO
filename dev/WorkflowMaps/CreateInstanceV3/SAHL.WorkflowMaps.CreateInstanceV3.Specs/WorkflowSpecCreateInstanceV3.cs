using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CreateInstanceV3;
using SAHL.Core.SystemMessages;
using SAHL.Core.Data.Models.X2;
using SAHL.WorkflowMaps.Specs.Common;

namespace SAHL.WorkflowMaps.CreateInstanceV3.Specs
{
    public class WorkflowSpecCreateInstanceV3 : WorkflowSpec<X2Create_Instance_V3, IX2Create_Instance_V3_Data>
    {  
        public WorkflowSpecCreateInstanceV3()
        {
            workflow = new X2Create_Instance_V3();
            workflowData = new X2Create_Instance_V3_Data();
        }
    }
}
