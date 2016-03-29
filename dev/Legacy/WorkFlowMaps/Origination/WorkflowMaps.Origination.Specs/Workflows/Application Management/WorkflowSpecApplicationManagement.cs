using Origination;
using WorkflowMaps.Specs.Common;

namespace WorkflowMaps.ApplicationManagement.Specs
{
    public class WorkflowSpecApplicationManagement : WorkflowSpec<X2Application_Management, IX2Application_Management_Data>
    {
        public WorkflowSpecApplicationManagement()
        {
            workflow = new X2Application_Management();
            workflowData = new X2Application_Management_Data();
        }
    }
}