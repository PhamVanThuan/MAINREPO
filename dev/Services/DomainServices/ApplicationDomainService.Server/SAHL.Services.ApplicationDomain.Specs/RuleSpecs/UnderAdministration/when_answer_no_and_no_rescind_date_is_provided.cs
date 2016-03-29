using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.RuleSpecs.UnderAdministration
{
    class when_answer_no_and_no_rescind_date_is_provided : WithFakes
    {
        static ApplicantDeclarationsModel ruleModel;
        static ISystemMessageCollection messages;
        static RescindedDateRequiredWhenClientHasBeenUnderAdminOrderRule rule;

        private Establish context = () =>
        {
            ruleModel = new ApplicantDeclarationsModel(1, 2, DateTime.Now,
                new DeclaredInsolventDeclarationModel(OfferDeclarationAnswer.No, DateTime.Now),
                new UnderAdministrationOrderDeclarationModel(OfferDeclarationAnswer.No, null),
                new CurrentlyUnderDebtCounsellingReviewDeclarationModel(OfferDeclarationAnswer.No, null),
                new PermissionToConductCreditCheckDeclarationModel(OfferDeclarationAnswer.Yes));
            messages = new SystemMessageCollection();

            rule = new RescindedDateRequiredWhenClientHasBeenUnderAdminOrderRule();
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, ruleModel);
        };

        private It should_not_contain_error_messages = () =>
        {
            messages.ErrorMessages().Count().ShouldEqual(0);
        };
    }
}
