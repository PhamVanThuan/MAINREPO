﻿using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.ClientDomain.Managers.DomiciliumAddress;
using SAHL.Services.ClientDomain.Rules;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System.Linq;

namespace SAHL.Services.ClientDomain.Specs.Rules.ClientAddressMustBeStreetAddressSpecs
{
    public class when_client_address_is_a_pending_domicilum_address : WithFakes
    {
        private static IDomiciliumAddressDataManager domiciliumDataManager;
        private static ClientAddressCannotBeAPendingDomiciliumAddressRule<ClientAddressAsPendingDomiciliumModel> rule;

        private static ClientAddressAsPendingDomiciliumModel ruleModel;
        private static ISystemMessageCollection messages;
        private static int clientAddressKey, clientKey;

        private Establish context = () =>
        {
            domiciliumDataManager = An<IDomiciliumAddressDataManager>();
            messages = SystemMessageCollection.Empty();
            clientAddressKey = 1312;
            clientKey = 1312;
            ruleModel = new ClientAddressAsPendingDomiciliumModel(clientAddressKey, clientKey);
            rule = new ClientAddressCannotBeAPendingDomiciliumAddressRule<ClientAddressAsPendingDomiciliumModel>(domiciliumDataManager);
            domiciliumDataManager.WhenToldTo(x => x.IsClientAddressPendingDomicilium(ruleModel.ClientAddresskey)).Return(true);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, ruleModel);
        };

        private It should_check_if_client_address_is_pending_domicilum = () =>
        {
            domiciliumDataManager.WasToldTo(x => x.IsClientAddressPendingDomicilium(ruleModel.ClientAddresskey));
        };

        private It should_contain_error_messages = () =>
        {
            messages.ErrorMessages().Count().ShouldNotEqual(0);
        };
    }
}