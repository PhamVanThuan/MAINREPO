using Loan_Adjustments;
using WorkflowMaps.Specs.Common;

namespace WorkflowMaps.LoanAdjustments.Specs
{
    public class WorkflowSpecLoanAdjustments : WorkflowSpec<X2Loan_Adjustments, IX2Loan_Adjustments_Data>
    {
        public WorkflowSpecLoanAdjustments()
        {
            workflow = new X2Loan_Adjustments();
            workflowData = new X2Loan_Adjustments_Data();
        }
    }
}