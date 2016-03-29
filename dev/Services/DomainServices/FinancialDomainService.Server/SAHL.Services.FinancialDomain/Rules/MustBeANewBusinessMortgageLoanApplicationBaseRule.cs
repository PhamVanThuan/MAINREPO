using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinancialDomain.Managers;
using SAHL.Services.Interfaces.FinancialDomain.Models;

namespace SAHL.Services.FinancialDomain.Rules
{
    public abstract class MustBeANewBusinessMortgageLoanApplicationBaseRule : IDomainRule<IApplicationModel>
    {
        private IFinancialDataManager financialDataManager;
        private string errorMessage;

        public MustBeANewBusinessMortgageLoanApplicationBaseRule(IFinancialDataManager financialDataManager, string errorMessage)
        {
            this.financialDataManager = financialDataManager;
            this.errorMessage = errorMessage;
        }

        public void ExecuteRule(Core.SystemMessages.ISystemMessageCollection messages, IApplicationModel ruleModel)
        {
            var offer = this.financialDataManager.GetApplication(ruleModel.ApplicationNumber);
            if (!((offer.OfferTypeKey == (int)OfferType.NewPurchaseLoan) || // new purchase
                (offer.OfferTypeKey == (int)OfferType.SwitchLoan) || // switch
                (offer.OfferTypeKey == (int)OfferType.RefinanceLoan) // refinance
                ))
            {
                messages.AddMessage(new SystemMessage(this.errorMessage, SystemMessageSeverityEnum.Error));
            }
        }
    }
}