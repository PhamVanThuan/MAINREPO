using Origination;
using WorkflowMaps.Specs.Common;

namespace WorkflowMaps.Credit.Specs
{
    public class WorkflowSpecCredit : WorkflowSpec<X2Credit, IX2Credit_Data>
    {
        public WorkflowSpecCredit()
        {
            workflow = new X2Credit();
            workflowData = new X2Credit_Data();
        }
    }
}