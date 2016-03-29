using Origination;
using WorkflowMaps.Specs.Common;

namespace WorkflowMaps.ReadvancePayments.Specs
{
    public class WorkflowSpecReadvancePayments : WorkflowSpec<X2Readvance_Payments, IX2Readvance_Payments_Data>
    {
        public WorkflowSpecReadvancePayments()
        {
            workflow = new X2Readvance_Payments();
            workflowData = new X2Readvance_Payments_Data();
        }
    }
}