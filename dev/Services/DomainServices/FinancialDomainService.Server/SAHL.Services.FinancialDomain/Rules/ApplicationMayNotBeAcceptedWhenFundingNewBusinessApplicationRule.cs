using SAHL.Services.FinancialDomain.Managers;

namespace SAHL.Services.FinancialDomain.Rules
{
    public class ApplicationMayNotBeAcceptedWhenFundingNewBusinessApplicationRule : ApplicationMayNotBeAcceptedBaseRule
    {
        public const string ErrorMessage = "Funding cannot be determined once the application has been accepted";

        public ApplicationMayNotBeAcceptedWhenFundingNewBusinessApplicationRule(IFinancialDataManager financialDataManager)
            : base(financialDataManager, ErrorMessage)
        {
        }
    }
}