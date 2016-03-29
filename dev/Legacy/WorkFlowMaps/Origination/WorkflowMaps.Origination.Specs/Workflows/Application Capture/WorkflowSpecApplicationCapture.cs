using Origination;
using WorkflowMaps.Specs.Common;

namespace WorkflowMaps.ApplicationCapture.Specs
{
    public class WorkflowSpecApplicationCapture : WorkflowSpec<X2Application_Capture, IX2Application_Capture_Data>
    {
        public WorkflowSpecApplicationCapture()
        {
            workflow = new X2Application_Capture();
            workflowData = new X2Application_Capture_Data();
        }
    }
}