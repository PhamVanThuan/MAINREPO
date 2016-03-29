using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.SystemMessages;
using SAHL.Services.AddressDomain.Managers;
using SAHL.Services.AddressDomain.Rules;
using SAHL.Services.Interfaces.AddressDomain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.AddressDomain.Specs.Rules.AddressCannotBeAnExistingClientAddress
{
    public class when_the_address_is_not_an_existing_client_address : WithFakes
    {
        private static AddressCannotBeAnExistingClientAddressRule rule;
        private static IAddressDataManager addressDataManager;
        private static ClientAddressModel clientAddressModel;
        private static LegalEntityAddressDataModel existingClientAddress;
        private static ISystemMessageCollection messages;

        private Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            existingClientAddress = null;
            clientAddressModel = new ClientAddressModel(12345, 6789, AddressType.Postal);
            addressDataManager = An<IAddressDataManager>();
            addressDataManager.WhenToldTo(x => x.GetExistingActiveClientAddress(clientAddressModel.ClientKey, clientAddressModel.AddressKey, clientAddressModel.AddressType))
                .Return(existingClientAddress);
            rule = new AddressCannotBeAnExistingClientAddressRule(addressDataManager);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, clientAddressModel);
        };

        private It should_not_return_a_message = () =>
        {
            messages.ErrorMessages().ShouldBeEmpty();
        };
    }
}