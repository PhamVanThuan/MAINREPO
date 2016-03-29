using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
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
using SAHL.Services.Interfaces.ClientDomain.Events;
using SAHL.Services.Interfaces.ClientDomain.Models;
using SAHL.Services.Interfaces.DomainQuery;
using System;
using System.Collections.Generic;

namespace SAHL.Services.ClientDomain.Specs.CommandHandlers.AddClientAddressAsPendingDomicilium
{
    public class when_an_event_is_raised : WithCoreFakes
    {
        private static IDomiciliumAddressDataManager domiciliumDataManager;
        private static IUnitOfWorkFactory uowFactory;
        private static IDomainRuleManager<ClientAddressAsPendingDomiciliumModel> domainRuleManager;
        private static IDomainQueryServiceClient domainQueryService;
        private static AddClientAddressAsPendingDomiciliumCommand command;
        private static AddClientAddressAsPendingDomiciliumCommandHandler handler;
        private static ClientAddressAsPendingDomiciliumModel clientAddressAsPendingDomicilium;
        private static IEnumerable<LegalEntityAddressDataModel> expectedClientAddress;
        private static IADUserManager adUserManager;
        private static int ADUserKey;
        private static int expectedDomiciliumAddresskey;
        private static int clientKey;
        private static int clientAddressKey;

        private Establish context = () =>
        {
            clientAddressKey = 123456;
            expectedClientAddress = new List<LegalEntityAddressDataModel>() { new LegalEntityAddressDataModel
                (121, 11, (int)AddressType.Residential, DateTime.Now, (int)GeneralStatus.Active) };
            domiciliumDataManager = An<IDomiciliumAddressDataManager>();
            uowFactory = An<IUnitOfWorkFactory>();
            domainRuleManager = An<IDomainRuleManager<ClientAddressAsPendingDomiciliumModel>>();
            domainQueryService = An<IDomainQueryServiceClient>();
            adUserManager = An<IADUserManager>();
            messages = SystemMessageCollection.Empty();
            expectedDomiciliumAddresskey = 2323;
            clientKey = 2323;
            clientAddressAsPendingDomicilium = new ClientAddressAsPendingDomiciliumModel(clientAddressKey, clientKey);
            command = new AddClientAddressAsPendingDomiciliumCommand(clientAddressAsPendingDomicilium, combGuid.Generate());
            handler = new AddClientAddressAsPendingDomiciliumCommandHandler
                (domiciliumDataManager, linkedKeyManager, domainRuleManager, combGuid, uowFactory, 
                 eventRaiser, domainQueryService, adUserManager);
            domiciliumDataManager.WhenToldTo
                (x => x.FindExistingActiveClientAddress
                    (command.ClientAddressAsPendingDomiciliumModel.ClientAddresskey)).Return(expectedClientAddress);
            ADUserKey = 1617;
            adUserManager.WhenToldTo(x => x.GetAdUserKeyByUserName(serviceRequestMetaData.UserName)).Return(ADUserKey);

            domiciliumDataManager.WhenToldTo
                (x => x.SavePendingDomiciliumAddress(Param.IsAny<LegalEntityDomiciliumDataModel>())).Return(expectedDomiciliumAddresskey);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_raise_the_correct_type_of_event = () =>
        {
            eventRaiser.WasToldTo
             (x => x.RaiseEvent(Param.IsAny<DateTime>(), Arg.Is<ClientAddressAsPendingDomiciliumAddedEvent>
              (y => y.LegalEntityAddressKey == clientAddressKey && 
                y.ClientDomiciliumKey == expectedDomiciliumAddresskey), expectedDomiciliumAddresskey,
                 (int)GenericKeyType.Address, serviceRequestMetaData));
        };
    }
}