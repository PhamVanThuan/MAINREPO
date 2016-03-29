using SAHL.Services.FinancialDomain.Managers;

namespace SAHL.Services.FinancialDomain.Rules
{
    public class ApplicationMustBeANewBusinessMortgageLoanWhenFundingRule : MustBeANewBusinessMortgageLoanApplicationBaseRule
    {
        public const string ErrorMessage = "When funding an application the application must be a new purchase application.";

        public ApplicationMustBeANewBusinessMortgageLoanWhenFundingRule(IFinancialDataManager financialDataManager)
            : base(financialDataManager, ErrorMessage)
        {
        }
    }
}