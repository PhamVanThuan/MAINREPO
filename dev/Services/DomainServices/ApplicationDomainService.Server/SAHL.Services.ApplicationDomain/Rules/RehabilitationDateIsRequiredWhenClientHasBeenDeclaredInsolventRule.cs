using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.ApplicationDomain.Models;

namespace SAHL.Services.ApplicationDomain.Rules
{
    public class RehabilitationDateIsRequiredWhenClientHasBeenDeclaredInsolventRule : IDomainRule<ApplicantDeclarationsModel>
    {
        public void ExecuteRule(ISystemMessageCollection messages, ApplicantDeclarationsModel ruleModel)
        {
            if (ruleModel.DeclaredInsolventDeclaration.Answer == OfferDeclarationAnswer.Date)
            {
                messages.AddMessage(new SystemMessage("Invalid answer was set. Expected answer was 'Yes or No', instead answer received was 'date'.", SystemMessageSeverityEnum.Error));
            }

            if (ruleModel.DeclaredInsolventDeclaration.Answer == OfferDeclarationAnswer.Yes && !ruleModel.DeclaredInsolventDeclaration.DateRehabilitated.HasValue)
            {
                messages.AddMessage(new SystemMessage("Client answered 'Yes' to the 'Have you ever been declared insolvent?' question. A date rehabilitated answer is required.", 
                    SystemMessageSeverityEnum.Error));
            }
            if (ruleModel.DeclaredInsolventDeclaration.Answer == OfferDeclarationAnswer.No && ruleModel.DeclaredInsolventDeclaration.DateRehabilitated.HasValue)
            {
                messages.AddMessage(new SystemMessage("Client answered 'No' to the 'Have you ever been declared insolvent?' question. A date rehabilitated answer should not be provided.", 
                    SystemMessageSeverityEnum.Error));
            }
        }
    }
}
