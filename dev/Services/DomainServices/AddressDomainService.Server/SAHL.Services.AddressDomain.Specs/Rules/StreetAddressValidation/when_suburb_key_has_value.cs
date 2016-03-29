using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.SystemMessages;
using rules = SAHL.Services.AddressDomain.Rules;
using System.Linq;

namespace SAHL.Services.AddressDomain.Specs.Rules.StreetAddressValidation
{
    public class when_suburb_key_has_value : WithFakes
    {
        private static rules.StreetAddressRequiresAValidSuburbRule domainRule;
        private static ISystemMessageCollection messages;
        private static AddressDataModel addressModel;

        private Establish context = () =>
        {
            domainRule = new rules.StreetAddressRequiresAValidSuburbRule();
            var suburbKey = 1;
            addressModel = new AddressDataModel(1, "", "", "", "", "", "", suburbKey, null, "", "", "", "", "", "", null, "", "", "", "", "", "");
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