using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;

namespace SAHL.Services.ApplicationDomain.Rules
{
    public class ApplicationMayNotBeAcceptedWhenSettingApplicationEmploymentTypeRule : IDomainRule<OfferInformationDataModel>
    {
        public void ExecuteRule(ISystemMessageCollection messages, OfferInformationDataModel ruleModel)
        {
            if (!ruleModel.OfferInformationTypeKey.HasValue) { return; }
            if (ruleModel.OfferInformationTypeKey.Value == (int)OfferInformationType.AcceptedOffer)
            {
                messages.AddMessage(new SystemMessage("Application employment type cannot be set once the application has been accepted", SystemMessageSeverityEnum.Error));
            }
        }
    }
}