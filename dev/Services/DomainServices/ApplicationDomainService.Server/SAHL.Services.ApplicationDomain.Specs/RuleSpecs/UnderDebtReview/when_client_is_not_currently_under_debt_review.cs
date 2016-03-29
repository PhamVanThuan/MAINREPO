using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.RuleSpecs.ApplicantDeclarations
{
    public class when_answer_is_yes_and_no_arrangement_is_set : WithFakes
    {
        static ApplicantDeclarationsModel ruleModel;
        static ISystemMessageCollection messages;
        static DebtArrangementAnsweredIsRequiredWhenUnderDebtReviewRule rule;

        private Establish context = () =>
        {
            ruleModel = new ApplicantDeclarationsModel(1, 2, DateTime.Now,
                new DeclaredInsolventDeclarationModel(OfferDeclarationAnswer.No, DateTime.Now),
                new UnderAdministrationOrderDeclarationModel(OfferDeclarationAnswer.No, null),
                new CurrentlyUnderDebtCounsellingReviewDeclarationModel(OfferDeclarationAnswer.Yes, null),
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