using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.ClientDomain.Managers.DomiciliumAddress;
using SAHL.Services.ClientDomain.Rules;
using SAHL.Services.Interfaces.ClientDomain.Models;

namespace SAHL.Services.ClientDomain.Specs.Rules.ClientAddressMustBeStreetAddressSpecs
{
    public class when_client_address_is_street_address : WithFakes
    {
        private static IDomiciliumAddressDataManager domiciliumDataManager;
        private static ClientAddressMustBeAStreetAddressRule<ClientAddressAsPendingDomiciliumModel> rule;

        private static ClientAddressAsPendingDomiciliumModel clientAddressAsPendingDomicilium;
        private static ISystemMessageCollection messages;
        private static int clientAddressKey, clientKey;

        private Establish context = () =>
        {
            domiciliumDataManager = An<IDomiciliumAddressDataManager>();
            messages = SystemMessageCollection.Empty();
            clientAddressKey = 1312;
            clientKey = 11;
            clientAddressAsPendingDomicilium = new ClientAddressAsPendingDomiciliumModel(clientAddressKey, clientKey);
            rule = new ClientAddressMustBeAStreetAddressRule<ClientAddressAsPendingDomiciliumModel>(domiciliumDataManager);
            domiciliumDataManager.WhenToldTo(x => x.CheckIsAddressTypeAResidentialAddress(clientAddressAsPendingDomicilium.ClientAddresskey)).Return(true);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, clientAddressAsPendingDomicilium);
        };

        private It should_not_return_messages = () =>
        {
            messages.ErrorMessages().ShouldBeEmpty();
        };

    }
}
