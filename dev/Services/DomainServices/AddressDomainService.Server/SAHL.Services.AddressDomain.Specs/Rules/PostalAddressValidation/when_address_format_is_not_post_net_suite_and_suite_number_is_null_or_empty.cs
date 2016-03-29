using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.SystemMessages;
using System.Linq;
using rules = SAHL.Services.AddressDomain.Rules;

namespace SAHL.Services.AddressDomain.Specs.Rules.PostalAddressValidation
{
    public class when_address_format_is_not_post_net_suite_and_suite_number_is_null_or_empty : WithFakes
    {
        private static rules.PostNetSuiteRequiresASuiteNumberRule domainRule;
        private static ISystemMessageCollection messages;
        private static AddressDataModel addressModel;

        private Establish context = () =>
        {
            domainRule = new rules.PostNetSuiteRequiresASuiteNumberRule();
            var addressFormatKey = (int)AddressFormat.Box;
            var suiteNumber = string.Empty;
            addressModel = new AddressDataModel(addressFormatKey, "", "", "", "", "", "", null, null, "", "", "", "", "", "", null, suiteNumber, "", "", "", "", "");
            messages = SystemMessageCollection.Empty();
        };

        private Because of = () =>
        {
            domainRule.ExecuteRule(messages, addressModel);
        };

        private It should_not_return_a_message = () =>
        {
            messages.AllMessages.Count<ISystemMessage>().ShouldEqual(0);
        };
    }
}