using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.ApplicationDomain.Models;

namespace SAHL.Services.ApplicationDomain.Rules
{
    public class DebtArrangementAnsweredIsRequiredWhenUnderDebtReviewRule : IDomainRule<ApplicantDeclarationsModel>
    {
        public void ExecuteRule(ISystemMessageCollection messages, ApplicantDeclarationsModel ruleModel)
        {
            if (ruleModel.CurrentlyUnderDebtReviewDeclaration.Answer == OfferDeclarationAnswer.Date)
            {
                messages.AddMessage(new SystemMessage("Invalid answer was set. Expected answer was 'Yes or No', instead answer received was 'date'.", SystemMessageSeverityEnum.Error));
            }

            if (ruleModel.CurrentlyUnderDebtReviewDeclaration.Answer == OfferDeclarationAnswer.Yes && !ruleModel.CurrentlyUnderDebtReviewDeclaration.HasCurrentDebtArrangement.HasValue)
            {
                messages.AddMessage(new SystemMessage(
                    @"Client answered 'Yes' to the 'Are you currently under debt counselling or review in terms of the National Credit Act, 2005?' question.
                    A do you currently have a debt rearrangement/s in place answer is required.", SystemMessageSeverityEnum.Error));
            }
            if (ruleModel.CurrentlyUnderDebtReviewDeclaration.Answer == OfferDeclarationAnswer.No && ruleModel.CurrentlyUnderDebtReviewDeclaration.HasCurrentDebtArrangement.HasValue)
            {
                messages.AddMessage(new SystemMessage(
                    @"Client answered 'No' to the 'Are you currently under debt counselling or review in terms of the National Credit Act, 2005?' question. 
                    A do you currently have a debt rearrangement/s in place answer should not be provided.", SystemMessageSeverityEnum.Error));
            }
        }
    }
}
