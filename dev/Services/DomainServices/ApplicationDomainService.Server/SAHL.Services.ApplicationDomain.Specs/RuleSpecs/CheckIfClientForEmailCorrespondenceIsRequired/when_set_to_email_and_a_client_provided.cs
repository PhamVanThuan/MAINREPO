using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Models;

namespace SAHL.Services.ApplicationDomain.Specs.RuleSpecs.CheckIfClientForEmailCorrespondenceIsRequired
{
    public class when_set_to_email_and_a_client_provided : WithFakes
    {
        private static CheckIfClientForEmailCorrespondenceIsRequiredRule rule;
        private static ISystemMessageCollection messages;
        private static ApplicationMailingAddressModel model;

        private Establish context = () =>
        {
            model = new ApplicationMailingAddressModel(1, 2, CorrespondenceLanguage.English, OnlineStatementFormat.PDFFormat,
                CorrespondenceMedium.Email, 1234567, true);
            messages = An<ISystemMessageCollection>();
            rule = new CheckIfClientForEmailCorrespondenceIsRequiredRule();
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, model);
        };

        private It should_not_return_messages = () =>
        {
            messages.ErrorMessages().ShouldBeEmpty();
        };
    }
}