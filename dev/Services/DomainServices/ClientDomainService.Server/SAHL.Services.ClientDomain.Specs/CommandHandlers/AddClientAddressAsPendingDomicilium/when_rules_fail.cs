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
using System.Linq;

namespace SAHL.Services.ClientDomain.Specs.CommandHandlers.AddClientAddressAsPendingDomicilium
{
    public class when_rules_fail : WithCoreFakes
    {
        private static IDomiciliumAddressDataManager domiciliumDataManager;
        private static IUnitOfWorkFactory uowFactory;
        private static IDomainRuleManager<ClientAddressAsPendingDomiciliumModel> domainRuleManager;
        private static IDomainQueryServiceClient domainQueryService;
        private static AddClientAddressAsPendingDomiciliumCommand command;
        private static AddClientAddressAsPendingDomiciliumCommandHandler handler;
        private static ClientAddressAsPendingDomiciliumModel clientAddressAsPendingDomicilium;
        private static IEnumerable<LegalEntityAddressDataModel> expectedClientAddress;
        private static int ADUserKey;
        private static int expectedDomiciliumAddresskey, clientKey;
        private static IADUserManager adUserManager;

        private Establish context = () =>
        {
            expectedClientAddress = new List<LegalEntityAddressDataModel>() { new LegalEntityAddressDataModel
                (121, 11, (int)AddressType.Residential, DateTime.Now, (int)GeneralStatus.Active) };
            domiciliumDataManager = An<IDomiciliumAddressDataManager>();
            uowFactory = An<IUnitOfWorkFactory>();
            domainRuleManager = An<IDomainRuleManager<ClientAddressAsPendingDomiciliumModel>>();
            domainQueryService = An<IDomainQueryServiceClient>();
            adUserManager = An<IADUserManager>();
            expectedDomiciliumAddresskey = 2323;
            clientKey = 2323;
            clientAddressAsPendingDomicilium = new ClientAddressAsPendingDomiciliumModel(121, clientKey);
            command = new AddClientAddressAsPendingDomiciliumCommand
                (clientAddressAsPendingDomicilium, combGuid.Generate());
            handler = new AddClientAddressAsPendingDomiciliumCommandHandler
                (domiciliumDataManager, linkedKeyManager, domainRuleManager, combGuid, 
                 uowFactory, eventRaiser, domainQueryService, adUserManager);
            domiciliumDataManager.WhenToldTo
                (x => x.FindExistingActiveClientAddress
                    (command.ClientAddressAsPendingDomiciliumModel.ClientAddresskey)).Return(expectedClientAddress);
            ADUserKey = 40;
            adUserManager.WhenToldTo
                (x => x.GetAdUserKeyByUserName(serviceRequestMetaData.UserName)).Return(ADUserKey);
            domiciliumDataManager.WhenToldTo
                (x => x.SavePendingDomiciliumAddress(Param.IsAny<LegalEntityDomiciliumDataModel>())).Return(expectedDomiciliumAddresskey);

            domainRuleManager.WhenToldTo
                (x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), 
                    command.ClientAddressAsPendingDomiciliumModel)).Callback<ISystemMessageCollection>
                       (y => y.AddMessage(new SystemMessage("rule failed error messages", SystemMessageSeverityEnum.Error)));
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_execute_the_rules = () =>
        {
            domainRuleManager.WasToldTo
                (x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), command.ClientAddressAsPendingDomiciliumModel));
        };

        private It should_not_retrieve_the_adUser = () =>
        {
            adUserManager.WasNotToldTo
                (x => x.GetAdUserKeyByUserName(serviceRequestMetaData.UserName));
        };

        private It should_contain_error_messages = () =>
        {
            messages.ErrorMessages().Count().ShouldNotEqual(0);
        };

        private It should_not_add_client_address_as_pending_domicilum = () =>
        {
            domiciliumDataManager.WasNotToldTo
                (x => x.SavePendingDomiciliumAddress(Param.IsAny<LegalEntityDomiciliumDataModel>()));
        };
    }
}