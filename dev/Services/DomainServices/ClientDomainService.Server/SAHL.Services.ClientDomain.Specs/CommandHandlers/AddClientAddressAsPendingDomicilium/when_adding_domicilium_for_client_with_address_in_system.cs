using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models._2AM.Managers.ADUser;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.ClientDomain.CommandHandlers;
using SAHL.Services.ClientDomain.Managers.DomiciliumAddress;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Models;
using SAHL.Services.Interfaces.DomainQuery;
using System;
using System.Collections.Generic;

namespace SAHL.Services.ClientDomain.Specs.CommandHandlers.AddClientAddressAsPendingDomicilium
{
    public class when_adding_domicilium_for_client_with_address_in_system : WithCoreFakes
    {
        private static AddClientAddressAsPendingDomiciliumCommandHandler handler;
        private static AddClientAddressAsPendingDomiciliumCommand command;
        private static ClientAddressAsPendingDomiciliumModel clientAddressAsPendingDomicilium;
        private static IDomiciliumAddressDataManager domiciliumDataManager;
        private static Guid clientDomiciliumGuid;
        private static IUnitOfWorkFactory uowFactory;
        private static int clientAddressKey;
        private static int expectedClientDomiciliumAddressKey;
        private static IDomainQueryServiceClient domainQueryService;
        private static IDomainRuleManager<ClientAddressAsPendingDomiciliumModel> domainRuleManager;
        private static IEnumerable<LegalEntityAddressDataModel> expectedClientAddress;
        private static IADUserManager adUserManager;
        private static int ADUserKey, clientKey;

        private Establish context = () =>
        {
            expectedClientAddress = new List<LegalEntityAddressDataModel>() { new LegalEntityAddressDataModel
                (121, 11, (int)AddressType.Residential, DateTime.Now, (int)GeneralStatus.Active) };
            uowFactory = An<IUnitOfWorkFactory>();
            domiciliumDataManager = An<IDomiciliumAddressDataManager>();
            domainRuleManager = An<IDomainRuleManager<ClientAddressAsPendingDomiciliumModel>>();
            domainQueryService = An<IDomainQueryServiceClient>();
            adUserManager = An<IADUserManager>();
            messages = SystemMessageCollection.Empty();
            clientDomiciliumGuid = combGuid.Generate();
            clientAddressKey = 321;
            expectedClientDomiciliumAddressKey = 3111;
            clientKey = 3111;
            clientAddressAsPendingDomicilium = new ClientAddressAsPendingDomiciliumModel(clientAddressKey, clientKey);
            command = new AddClientAddressAsPendingDomiciliumCommand(clientAddressAsPendingDomicilium, clientDomiciliumGuid);
            domiciliumDataManager.WhenToldTo(x => x.FindExistingActiveClientAddress(clientAddressKey)).Return(expectedClientAddress);
            domiciliumDataManager.WhenToldTo(x => x.SavePendingDomiciliumAddress(Param.IsAny<LegalEntityDomiciliumDataModel>()))
                .Return(expectedClientDomiciliumAddressKey);
            handler = new AddClientAddressAsPendingDomiciliumCommandHandler
                (domiciliumDataManager, linkedKeyManager, domainRuleManager, combGuid, uowFactory, eventRaiser,
                 domainQueryService, adUserManager);

            ADUserKey = 1617;
            adUserManager.WhenToldTo(x => x.GetAdUserKeyByUserName(serviceRequestMetaData.UserName)).Return(ADUserKey);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        It should_retrieve_the_aduser_key = () =>
        {
            adUserManager.WasToldTo(x => x.GetAdUserKeyByUserName(serviceRequestMetaData.UserName));
        };

        private It should_add_client_address_as_pending_domicilium = () =>
        {
            domiciliumDataManager.WasToldTo(x => x.SavePendingDomiciliumAddress(Param.IsAny<LegalEntityDomiciliumDataModel>()));
        };

        private It should_link_generated_domicilium_address_key_to_guid = () =>
        {
            linkedKeyManager.WasToldTo(x => x.LinkKeyToGuid(expectedClientDomiciliumAddressKey, clientDomiciliumGuid));
        };
        private It should_not_return_any_messages = () =>
        {
            messages.ErrorMessages().ShouldBeEmpty();
        };
    }
}
