using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.RuleSpecs.CheckIfClientForEmailCorrespondenceIsRequired
{
    public class when_set_to_email_and_client_is_null : WithFakes
    {
        private static CheckIfClientForEmailCorrespondenceIsRequiredRule rule;
        private static ISystemMessageCollection messages;
        private static ApplicationMailingAddressModel model;

        private Establish context = () =>
        {
            model = new ApplicationMailingAddressModel(1, 2, CorrespondenceLanguage.English, OnlineStatementFormat.PDFFormat,
                CorrespondenceMedium.Email, null, true);
            messages = SystemMessageCollection.Empty();
            rule = new CheckIfClientForEmailCorrespondenceIsRequiredRule();
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, model);
        };

        private It should_not_return_messages = () =>
        {
            messages.ErrorMessages().First().Message
                .ShouldContain(@"A client to use for email correspondence is required when the correspondence medium
                                has been set to Email.");
        };
    }
}