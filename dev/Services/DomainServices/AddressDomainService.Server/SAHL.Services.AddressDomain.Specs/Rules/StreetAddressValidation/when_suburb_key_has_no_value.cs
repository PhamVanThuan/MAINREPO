using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.SystemMessages;
using rules = SAHL.Services.AddressDomain.Rules;

namespace SAHL.Services.AddressDomain.Specs.Rules.StreetAddressValidation
{
    public class when_suburb_key_has_no_value : WithFakes
    {
        private static rules.StreetAddressRequiresAValidSuburbRule domainRule;
        private static ISystemMessageCollection messages;
        private static AddressDataModel addressModel;

        private Establish context = () =>
        {
            domainRule = new rules.StreetAddressRequiresAValidSuburbRule();
            addressModel = new AddressDataModel(1, "", "", "", "", "", "", null, null, "", "", "", "", "", "", null, "", "", "", "", "", "");
            messages = SystemMessageCollection.Empty();
        };

        private Because of = () =>
        {
            domainRule.ExecuteRule(messages, addressModel);
        };

        private It should_return_a_message = () =>
        {
            messages.AllMessages.ShouldContain<ISystemMessage>(x => x.Message == "No matching suburb could be found for the address." && x.Severity == SystemMessageSeverityEnum.Error);
        };
    }
}