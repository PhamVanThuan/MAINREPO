using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.ApplicationDomain.Models;

namespace SAHL.Services.ApplicationDomain.Rules
{
    public class RescindedDateRequiredWhenClientHasBeenUnderAdminOrderRule : IDomainRule<ApplicantDeclarationsModel>
    {
        public void ExecuteRule(Core.SystemMessages.ISystemMessageCollection messages, ApplicantDeclarationsModel ruleModel)
        {
            if (ruleModel.UnderAdministrationOrderDeclaration.Answer == OfferDeclarationAnswer.Date)
            {
                messages.AddMessage(new SystemMessage("Invalid answer was set. Expected answer was 'Yes or No', instead answer received was 'date'.", SystemMessageSeverityEnum.Error));
            }

            if (ruleModel.UnderAdministrationOrderDeclaration.Answer == OfferDeclarationAnswer.Yes && !ruleModel.UnderAdministrationOrderDeclaration.DateAdministrationOrderRescinded.HasValue)
            {
                messages.AddMessage(new SystemMessage(@"Client answered 'Yes' to the 'Have you ever been under administration order?' question. 
                A date administration order rescinded answer is required.", SystemMessageSeverityEnum.Error));
            }
            if (ruleModel.UnderAdministrationOrderDeclaration.Answer == OfferDeclarationAnswer.No && ruleModel.UnderAdministrationOrderDeclaration.DateAdministrationOrderRescinded.HasValue)
            {
                messages.AddMessage(new SystemMessage(@"Client answered 'No' to the 'Have you ever been under administration order?' question. 
                A date administration order rescinded should not be provided.", SystemMessageSeverityEnum.Error));
            }
        }
    }
}
