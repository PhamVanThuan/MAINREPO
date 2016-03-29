﻿using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.ClientDomain.Managers.DomiciliumAddress;
using SAHL.Services.ClientDomain.Rules;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System.Linq;

namespace SAHL.Services.ClientDomain.Specs.Rules.ClientAddressCannotBeAnActiveDomiciliumSpecs
{
    public class when_client_address_is_not_an_active_domicilium_on_the_client : WithFakes
    {
        static IDomiciliumAddressDataManager domiciliumAddressDataManager;
        static ClientAddressCannotBeAnActiveDomiciliumAddressRule<ClientAddressAsPendingDomiciliumModel> rule;
        static ClientAddressAsPendingDomiciliumModel ruleModel;
        static ISystemMessageCollection messages;
        static int clientAddressKey, clientKey;

        Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            clientAddressKey = 1111;
            clientKey = 5050;
            ruleModel = new ClientAddressAsPendingDomiciliumModel(clientAddressKey, clientKey);
            domiciliumAddressDataManager = An<IDomiciliumAddressDataManager>();
            rule = new ClientAddressCannotBeAnActiveDomiciliumAddressRule<ClientAddressAsPendingDomiciliumModel>(domiciliumAddressDataManager);
            domiciliumAddressDataManager.WhenToldTo(x => x.IsClientAddressActiveDomicilium(ruleModel.ClientAddresskey, ruleModel.ClientKey)).Return(false);
        };

        Because of = () =>
        {
            rule.ExecuteRule(messages, ruleModel);
        };

        It should_check_client_address_is_active_domicilim_on_client = () =>
        {
            domiciliumAddressDataManager.WasToldTo(x => x.IsClientAddressActiveDomicilium(clientAddressKey, clientKey));
        };

        It should_not_contain_error_messages = () =>
        {
            messages.ErrorMessages().Count().ShouldEqual(0);
        };
    }
}