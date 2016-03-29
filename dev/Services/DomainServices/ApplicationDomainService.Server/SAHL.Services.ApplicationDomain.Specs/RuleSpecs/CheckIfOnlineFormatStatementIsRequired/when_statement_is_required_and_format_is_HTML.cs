using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Models;

namespace SAHL.Services.ApplicationDomain.Specs.RuleSpecs.CheckIfOnlineFormatStatementIsRequired
{
    public class when_statement_is_required_and_format_is_HTML : WithFakes
    {
        private static CheckIfOnlineFormatStatementIsRequiredRule rule;
        private static ISystemMessageCollection messages;
        private static ApplicationMailingAddressModel model;

        private Establish context = () =>
        {
            messages = An<ISystemMessageCollection>();
            rule = new CheckIfOnlineFormatStatementIsRequiredRule();
            model = new ApplicationMailingAddressModel(1, 2, CorrespondenceLanguage.English, OnlineStatementFormat.HTML, CorrespondenceMedium.Email,
                1, true);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, model);
        };

        private It should_not_return_error_messages = () =>
        {
            messages.ErrorMessages().ShouldBeEmpty();
        };
    }
}