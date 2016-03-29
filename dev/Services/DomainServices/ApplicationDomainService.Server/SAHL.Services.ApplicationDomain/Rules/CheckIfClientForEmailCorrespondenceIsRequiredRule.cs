using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.ApplicationDomain.Models;

namespace SAHL.Services.ApplicationDomain.Rules
{
    public class CheckIfClientForEmailCorrespondenceIsRequiredRule : IDomainRule<ApplicationMailingAddressModel>
    {
        public void ExecuteRule(Core.SystemMessages.ISystemMessageCollection messages, ApplicationMailingAddressModel ruleModel)
        {
            if (ruleModel.CorrespondenceMedium == CorrespondenceMedium.Email)
            {
                if (ruleModel.ClientToUseForEmailCorrespondence == null)
                {
                    messages.AddMessage(new SystemMessage(@"A client to use for email correspondence is required when the correspondence medium
                                has been set to Email.", SystemMessageSeverityEnum.Error));
                }
                if (ruleModel.ClientToUseForEmailCorrespondence == 0)
                {
                    messages.AddMessage(new SystemMessage(@"A client to use for email correspondence is required when the correspondence medium
                                has been set to Email.", SystemMessageSeverityEnum.Error));
                }
            }

            if (ruleModel.CorrespondenceMedium == CorrespondenceMedium.Post && (ruleModel.ClientToUseForEmailCorrespondence != null || ruleModel.ClientToUseForEmailCorrespondence > 0))
            {
                messages.AddMessage(new SystemMessage(@"A client for email correspondence is not required when the correspondence medium is Post.", SystemMessageSeverityEnum.Error));
            }
        }
    }
}