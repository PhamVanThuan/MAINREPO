using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.RuleSpecs.UnderDebtReview
{
    public class when_the_answer_is_date : WithFakes
    {
        static ApplicantDeclarationsModel ruleModel;
        static ISystemMessageCollection messages;
        static DebtArrangementAnsweredIsRequiredWhenUnderDebtReviewRule rule;

        private Establish context = () =>
        {
            ruleModel = new ApplicantDeclarationsModel(1, 2, DateTime.Now,
                new DeclaredInsolventDeclarationModel(OfferDeclarationAnswer.Yes, DateTime.Now),
                new UnderAdministrationOrderDeclarationModel(OfferDeclarationAnswer.Yes, DateTime.Now),
                new CurrentlyUnderDebtCounsellingReviewDeclarationModel(OfferDeclarationAnswer.Date, null),
                new PermissionToConductCreditCheckDeclarationModel(OfferDeclarationAnswer.Yes));
            messages = new SystemMessageCollection();

            rule = new DebtArrangementAnsweredIsRequiredWhenUnderDebtReviewRule();
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, ruleModel);
        };

        private It should_contain_error_messages = () =>
        {
            messages.ErrorMessages().Count().ShouldNotEqual(0);
        };
    }
}
