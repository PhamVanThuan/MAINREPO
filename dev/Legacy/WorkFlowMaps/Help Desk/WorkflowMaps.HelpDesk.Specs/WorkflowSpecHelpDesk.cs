using Help_Desk;
using WorkflowMaps.Specs.Common;

namespace WorkflowMaps.HelpDesk.Specs
{
    public class WorkflowSpecHelpDesk : WorkflowSpec<X2Help_Desk, IX2Help_Desk_Data>
    {
        public WorkflowSpecHelpDesk()
        {
            workflow = new X2Help_Desk();
            workflowData = new X2Help_Desk_Data();
        }
    }
}