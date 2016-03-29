﻿using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.SystemMessages;
using SAHL.Services.ClientDomain.Managers.DomiciliumAddress;
using SAHL.Services.ClientDomain.Rules;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.ClientDomain.Specs.Rules.AddressMustBeAnActiveClientAddress
{
    public class when_client_address_is_not_active : WithFakes
    {
        private static ISystemMessageCollection messages;
        private static IDomiciliumAddressDataManager domiciliumAddressDataManager;
        private static AddressMustBeAnActiveClientAddressRule rule;
        private static ClientAddressAsPendingDomiciliumModel ruleModel;
        private static int clientAddressKey, clientKey;
        private static IEnumerable<LegalEntityAddressDataModel> expectedAddressList;

        private Establish context = () =>
        {
            clientAddressKey = 314;
            clientKey = 2718;
            domiciliumAddressDataManager = An<IDomiciliumAddressDataManager>();
            messages = SystemMessageCollection.Empty();
            ruleModel = new ClientAddressAsPendingDomiciliumModel(clientAddressKey, clientKey);
            rule = new AddressMustBeAnActiveClientAddressRule(domiciliumAddressDataManager);

            expectedAddressList = new List<LegalEntityAddressDataModel>();
            domiciliumAddressDataManager.WhenToldTo(x => x.FindExistingActiveClientAddress(ruleModel.ClientAddresskey)).Return(expectedAddressList);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, ruleModel);
        };

        private It should_check_if_client_address_is_ative = () =>
        {
            domiciliumAddressDataManager.WasToldTo(x => x.FindExistingActiveClientAddress(ruleModel.ClientAddresskey));
        };

        private It should_return_error_messages = () =>
        {
            messages.ErrorMessages().Count().ShouldNotEqual(0);
        };
    }
}