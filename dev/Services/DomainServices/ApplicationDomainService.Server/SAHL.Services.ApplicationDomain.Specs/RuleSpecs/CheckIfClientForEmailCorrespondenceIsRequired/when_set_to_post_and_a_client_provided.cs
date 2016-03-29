using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.RuleSpecs.CheckIfClientForEmailCorrespondenceIsRequired
{
    public class when_set_to_post_and_a_client_provided : WithFakes
    {
        private static CheckIfClientForEmailCorrespondenceIsRequiredRule rule;
        private static ISystemMessageCollection messages;
        private static ApplicationMailingAddressModel model;

        private Establish context = () =>
        {
            model = new ApplicationMailingAddressModel(1, 2, CorrespondenceLanguage.English, OnlineStatementFormat.PDFFormat,
                CorrespondenceMedium.Post, 1234567, true);
            messages = SystemMessageCollection.Empty();
            rule = new CheckIfClientForEmailCorrespondenceIsRequiredRule();
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, model);
        };

        private It should_return_messages = () =>
        {
            messages.ErrorMessages().First().Message
                .ShouldEqual("A client for email correspondence is not required when the correspondence medium is Post.");
        };
    }
}