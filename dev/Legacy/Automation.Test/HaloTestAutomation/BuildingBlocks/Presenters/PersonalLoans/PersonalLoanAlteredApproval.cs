using BuildingBlocks.Services.Contracts;
using ObjectMaps.Presenters.PersonalLoans;

namespace BuildingBlocks.Presenters.PersonalLoans
{
    public class PersonalLoanAlteredApproval : PersonalLoanAlteredApprovalControls
    {
        private readonly ICommonService commonService;

        public PersonalLoanAlteredApproval()
        {
            commonService = ServiceLocator.Instance.GetService<ICommonService>();
        }

        public void SelectOptionFromGrid(int term)
        {
            var row = base.GetTableRowByTermValue(term);
            row.Click();
        }

        public void Calculate(int term, decimal amount)
        {
            string rands, cents;
            if (term > 0)
                base.txtLoanTerm.Value = term.ToString();
            commonService.SplitRandsCents(out rands, out cents, amount.ToString());
            base.txtLoanAmountRands.Value = rands;
            base.txtLoanAmountCents.Value = cents;
            base.btnCalculate.Click();
        }

        public void UpdateApplication()
        {
            base.UpdateApplication.Click();
        }
    }
}