using SAHL.Services.FinancialDomain.Managers;

namespace SAHL.Services.FinancialDomain.Rules
{
    public class ApplicationMayNotBeAcceptedWhenPricingNewBusinessApplicationRule : ApplicationMayNotBeAcceptedBaseRule
    {
        public const string ErrorMessage = "Pricing cannot be determined once the application has been accepted";

        public ApplicationMayNotBeAcceptedWhenPricingNewBusinessApplicationRule(IFinancialDataManager financialDataManager)
            : base(financialDataManager, ErrorMessage)
        {
        }
    }
}