using CAP2_Offers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkflowMaps.Specs.Common;

namespace WorkflowMaps.Cap2.Specs
{
    public class WorkflowSpecCap2 : WorkflowSpec<X2CAP2_Offers, IX2CAP2_Offers_Data>
    {
        public WorkflowSpecCap2()
        {
            workflow = new X2CAP2_Offers();
            workflowData = new X2CAP2_Offers_Data();
        }
    }
}