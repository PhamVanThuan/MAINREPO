using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.RuleSpecs.CheckIfOnlineFormatStatementIsRequired
{
    public class when_statement_is_required_and_format_is_NA : WithFakes
    {
        private static CheckIfOnlineFormatStatementIsRequiredRule rule;
        private static ISystemMessageCollection messages;
        private static ApplicationMailingAddressModel model;

        private Establish context = () =>
        {
            messages = new SystemMessageCollection();
            rule = new CheckIfOnlineFormatStatementIsRequiredRule();
            model = new ApplicationMailingAddressModel(1, 2, CorrespondenceLanguage.English, OnlineStatementFormat.NotApplicable, CorrespondenceMedium.Email,
                1, true);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, model);
        };

        private It should_return_a_message = () =>
        {
            messages.ErrorMessages().First().Message
                .ShouldContain("The format of the online statement must be selected when an online statement is required.");
        };
    }
}