using SAHL.Services.FinancialDomain.Managers;

namespace SAHL.Services.FinancialDomain.Rules
{
    public class ApplicationMustBeANewBusinessMortgageLoanWhenPricingRule : MustBeANewBusinessMortgageLoanApplicationBaseRule
    {
        public const string ErrorMessage = "When pricing an application the application must be a new purchase application.";

        public ApplicationMustBeANewBusinessMortgageLoanWhenPricingRule(IFinancialDataManager financialDataManager)
            : base(financialDataManager, ErrorMessage)
        {
        }
    }
}