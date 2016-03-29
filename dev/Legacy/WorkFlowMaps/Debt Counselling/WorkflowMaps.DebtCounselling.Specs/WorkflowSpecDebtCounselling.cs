using Debt_Counselling;
using WorkflowMaps.Specs.Common;

namespace WorkflowMaps.DebtCounselling.Specs
{
    public class WorkflowSpecDebtCounselling : WorkflowSpec<X2Debt_Counselling, IX2Debt_Counselling_Data>
    {
        public WorkflowSpecDebtCounselling()
        {
            workflow = new X2Debt_Counselling();
            workflowData = new X2Debt_Counselling_Data();
        }
    }
}