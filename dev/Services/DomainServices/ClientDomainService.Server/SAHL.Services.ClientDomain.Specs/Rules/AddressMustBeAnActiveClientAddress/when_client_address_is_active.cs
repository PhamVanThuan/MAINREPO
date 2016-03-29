﻿using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.SystemMessages;
using SAHL.Services.ClientDomain.Managers.DomiciliumAddress;
using SAHL.Services.ClientDomain.Rules;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.ClientDomain.Specs.Rules.AddressMustBeAnActiveClientAddress
{
    public class when_client_address_is_active : WithFakes
    {
        static ISystemMessageCollection messages;
        static IDomiciliumAddressDataManager domiciliumAddressDataManager;
        static AddressMustBeAnActiveClientAddressRule rule;
        static ClientAddressAsPendingDomiciliumModel ruleModel;
        static int addressKey, clientAddressKey, clientKey;
        static IEnumerable<LegalEntityAddressDataModel> expectedAddressList;

        Establish context = () =>
        {
            clientAddressKey = 314;
            clientKey = 2718;
            addressKey = 21368;
            domiciliumAddressDataManager = An<IDomiciliumAddressDataManager>();
            messages = SystemMessageCollection.Empty();
            ruleModel = new ClientAddressAsPendingDomiciliumModel(clientAddressKey, clientKey);
            rule = new AddressMustBeAnActiveClientAddressRule(domiciliumAddressDataManager);

            expectedAddressList = new List<LegalEntityAddressDataModel> { new LegalEntityAddressDataModel(clientKey, addressKey, 
                                                                                                         (int)AddressType.Residential, DateTime.Now, 
                                                                                                         (int)GeneralStatus.Active) };
            domiciliumAddressDataManager.WhenToldTo
                (x => x.FindExistingActiveClientAddress
                    (ruleModel.ClientAddresskey)).Return(expectedAddressList);
        };

        Because of = () =>
        {
            rule.ExecuteRule(messages, ruleModel);
        };

        It should_check_if_client_address_is_ative = () =>
        {
            domiciliumAddressDataManager.WasToldTo
                (x => x.FindExistingActiveClientAddress
                    (ruleModel.ClientAddresskey));
        };

        It should_not_return_error_messages = () =>
        {
            messages.ErrorMessages().Count().ShouldEqual(0);
        };
    }
}