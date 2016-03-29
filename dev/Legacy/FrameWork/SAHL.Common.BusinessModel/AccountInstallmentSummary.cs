using SAHL.Common.BusinessModel.Helpers;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    public class AccountInstallmentSummary : IAccountInstallmentSummary
    {
        IAccount _account;

        public AccountInstallmentSummary(IAccount account)
        {
            _account = account;
        }

        public double TotalLoanInstallment
        {
            get { return MortgageLoanAccountHelper.TotalLoanInstallment(_account); }
        }

        public double TotalShortTermLoanInstallment
        {
            get { return MortgageLoanAccountHelper.TotalShortTermLoanInstallment(_account); }
        }

        public double TotalPremium
        {
            get { return MortgageLoanAccountHelper.TotalPremium(_account); }
        }

        public double MonthlyServiceFee
        {
            get { return MortgageLoanAccountHelper.MonthlyServiceFee(_account); }
        }

        public double TotalAmountDue
        {
            get { return MortgageLoanAccountHelper.TotalAmountDue(_account); }
        }

        public double MonthsInArrears
        {
            get { return MortgageLoanAccountHelper.MonthsInArrears(_account); }
        }

        public double AmortisingInstallment
        {
            get { return MortgageLoanAccountHelper.AmortisingInstallment(_account); }
        }

        public bool IsInterestOnly
        {
            get { return MortgageLoanAccountHelper.IsInterestOnly(_account); }
        }

        public double TotalArrearsBalance
        {
            get { return MortgageLoanAccountHelper.TotalArrearBalance(_account); }
        }

        public double TotalRegentPremium
        {
            get { return MortgageLoanAccountHelper.GetRegentPremium(_account); }
        }

        public double CurrentBalance
        {
            get { return MortgageLoanAccountHelper.GetCurrentBalance(_account); }
        }
    }
}