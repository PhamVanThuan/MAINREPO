using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.SystemMessages;
using rules = SAHL.Services.AddressDomain.Rules;
using System.Linq;
using SAHL.Core.BusinessModel.Enums;

namespace SAHL.Services.AddressDomain.Specs.Rules.PostalAddressValidation
{
    public class when_address_format_is_post_net_suite_and_suite_Number_is_null_or_empty : WithFakes
    {
        private static rules.PostNetSuiteRequiresASuiteNumberRule domainRule;
        private static ISystemMessageCollection messages;
        private static AddressDataModel addressModel;

        private Establish context = () =>
        {
            domainRule = new rules.PostNetSuiteRequiresASuiteNumberRule();
            var addressFormatKey = (int)AddressFormat.PostNetSuite;
            var suiteNumber = string.Empty;
            addressModel = new AddressDataModel(addressFormatKey, "", "", "", "", "", "", null, null, "", "", "", "", "", "", null, suiteNumber, "", "", "", "", "");
            messages = SystemMessageCollection.Empty();
        };

        private Because of = () =>
        {
            domainRule.ExecuteRule(messages, addressModel);
        };

        private It should_return_a_message = () =>
        {
            messages.AllMessages.ShouldContain<ISystemMessage>(x => x.Message == "A postnet suite address requires a postnet suite number." && x.Severity == SystemMessageSeverityEnum.Error);
        };
    }
}
