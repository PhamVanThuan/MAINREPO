using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.RuleSpecs.CheckIfOnlineFormatStatementIsRequired
{
    public class when_statement_is_not_required_and_format_is_PDF : WithFakes
    {
        private static CheckIfOnlineFormatStatementIsRequiredRule rule;
        private static ISystemMessageCollection messages;
        private static ApplicationMailingAddressModel model;

        private Establish context = () =>
        {
            messages = new SystemMessageCollection();
            rule = new CheckIfOnlineFormatStatementIsRequiredRule();
            model = new ApplicationMailingAddressModel(1, 2, CorrespondenceLanguage.English, OnlineStatementFormat.PDFFormat, CorrespondenceMedium.Email,
                1, false);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, model);
        };

        private It should_return_a_message = () =>
        {
            messages.ErrorMessages().First().Message
                .ShouldContain("An online statement format is not required when the option for the online statement has not been selected.");
        };
    }
}