using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.ClientDomain.Managers.DomiciliumAddress;
using SAHL.Services.ClientDomain.Rules;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System.Linq;

namespace SAHL.Services.ClientDomain.Specs.Rules.ClientAddressMustBeStreetAddressSpecs
{
    public class when_client_address_is_not_street_address : WithFakes
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
            clientKey = 1312;
            clientAddressAsPendingDomicilium = new ClientAddressAsPendingDomiciliumModel(clientAddressKey, clientKey);
            rule = new ClientAddressMustBeAStreetAddressRule<ClientAddressAsPendingDomiciliumModel>(domiciliumDataManager);
            domiciliumDataManager.WhenToldTo(x => x.CheckIsAddressTypeAResidentialAddress(clientAddressAsPendingDomicilium.ClientAddresskey)).Return(false);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, clientAddressAsPendingDomicilium);
        };

        private It should_check_if_address_type_is_postal = () =>
        {
            domiciliumDataManager.WasToldTo(x => x.CheckIsAddressTypeAResidentialAddress(clientAddressAsPendingDomicilium.ClientAddresskey));
        };

        private It should_return_message = () =>
        {
            messages.ErrorMessages().First().Message.ShouldContain("Client address must be a street address.");
        };
    }
}
