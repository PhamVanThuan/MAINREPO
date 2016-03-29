using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinancialDomain.Managers;
using SAHL.Services.Interfaces.FinancialDomain.Models;

namespace SAHL.Services.FinancialDomain.Rules
{
    public abstract class ApplicationMayNotBeAcceptedBaseRule : IDomainRule<IApplicationModel>
    {
        private IFinancialDataManager financialDataManager;
        private string errorMessage;

        public ApplicationMayNotBeAcceptedBaseRule(IFinancialDataManager financialDataManager, string errorMessage)
        {
            this.financialDataManager = financialDataManager;
            this.errorMessage = errorMessage;
        }

        public void ExecuteRule(ISystemMessageCollection messages, IApplicationModel ruleModel)
        {
            OfferInformationDataModel offerInformation = this.financialDataManager.GetLatestApplicationOfferInformation(ruleModel.ApplicationNumber);
            if (!offerInformation.OfferInformationTypeKey.HasValue) { return; }
            if (offerInformation.OfferInformationTypeKey.Value == (int)OfferInformationType.AcceptedOffer)
            {
                messages.AddMessage(new SystemMessage(this.errorMessage, SystemMessageSeverityEnum.Error));
            }
        }
    }
}