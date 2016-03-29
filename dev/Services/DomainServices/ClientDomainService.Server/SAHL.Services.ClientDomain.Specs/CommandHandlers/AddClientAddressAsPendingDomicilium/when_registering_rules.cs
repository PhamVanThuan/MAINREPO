﻿using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM.Managers.ADUser;
using SAHL.Core.Rules;
using SAHL.Core.Testing;
using SAHL.Services.ClientDomain.CommandHandlers;
using SAHL.Services.ClientDomain.Managers.DomiciliumAddress;
using SAHL.Services.ClientDomain.Rules;
using SAHL.Services.Interfaces.ClientDomain.Models;
using SAHL.Services.Interfaces.DomainQuery;

namespace SAHL.Services.ClientDomain.Specs.CommandHandlers.AddClientAddressAsPendingDomicilium
{
    public class when_registering_rules : WithCoreFakes
    {
        private static IDomiciliumAddressDataManager domiciliumDataManager;
        private static IUnitOfWorkFactory uowFactory;
        private static IDomainRuleManager<ClientAddressAsPendingDomiciliumModel> domainRuleManager;
        private static IDomainQueryServiceClient domainQueryService;
        private static AddClientAddressAsPendingDomiciliumCommandHandler handler;
        private static IADUserManager adUserManager;

        Establish context = () =>
        {
            domiciliumDataManager = An<IDomiciliumAddressDataManager>();
            uowFactory = An<IUnitOfWorkFactory>();
            domainRuleManager = An<IDomainRuleManager<ClientAddressAsPendingDomiciliumModel>>();
            domainQueryService = An<IDomainQueryServiceClient>();
            adUserManager = An<IADUserManager>();
        };

        Because of = () =>
        {
            handler = new AddClientAddressAsPendingDomiciliumCommandHandler
             (domiciliumDataManager, linkedKeyManager, domainRuleManager, 
               combGuid, uowFactory, eventRaiser, domainQueryService, adUserManager);
        };

        It should_register_the_ClientAddressMustBeAStreetAddressRule = () =>
        {
            domainRuleManager.WasToldTo
                (x => x.RegisterRule(Param.IsAny<ClientAddressMustBeAStreetAddressRule<ClientAddressAsPendingDomiciliumModel>>()));
        };

        It should_register_the_AddressMustBeAnActiveClientAddressRule = () =>
        {
            domainRuleManager.WasToldTo
                (x => x.RegisterRule(Param.IsAny<AddressMustBeAnActiveClientAddressRule>()));
        };
    }
}