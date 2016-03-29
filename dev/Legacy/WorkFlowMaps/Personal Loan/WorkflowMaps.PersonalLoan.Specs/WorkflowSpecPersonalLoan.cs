using Personal_Loan;
using WorkflowMaps.Specs.Common;

namespace WorkflowMaps.PersonalLoan.Specs
{
    public class WorkflowSpecPersonalLoans : WorkflowSpec<X2Personal_Loans, IX2Personal_Loans_Data>
    {
        public WorkflowSpecPersonalLoans()
        {
            workflow = new X2Personal_Loans();
            workflowData = new X2Personal_Loans_Data();
        }
    }
}