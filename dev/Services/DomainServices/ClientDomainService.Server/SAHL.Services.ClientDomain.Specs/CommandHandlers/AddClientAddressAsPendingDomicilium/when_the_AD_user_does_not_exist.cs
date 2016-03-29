using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models._2AM.Managers.ADUser;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
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
    public class when_the_AD_user_does_not_exist : WithCoreFakes
    {
        private static AddClientAddressAsPendingDomiciliumCommandHandler handler;
        private static AddClientAddressAsPendingDomiciliumCommand command;
        private static ClientAddressAsPendingDomiciliumModel clientAddressAsPendingDomicilium;
        private static IDomiciliumAddressDataManager domiciliumDataManager;
        private static Guid clientDomiciliumGuid;
        private static int clientAddressKey;
        private static IDomainQueryServiceClient domainQueryService;
        private static IDomainRuleManager<ClientAddressAsPendingDomiciliumModel> domainRuleManager;
        private static IEnumerable<LegalEntityAddressDataModel> expectedClientAddress;
        private static IADUserManager adUserManager;
        private static int ADUserKey;

        private Establish context = () =>
        {
            expectedClientAddress = new List<LegalEntityAddressDataModel>() { new LegalEntityAddressDataModel
                (121, 11, (int)AddressType.Residential, DateTime.Now, (int)GeneralStatus.Active) };
            domiciliumDataManager = An<IDomiciliumAddressDataManager>();
            domainRuleManager = An<IDomainRuleManager<ClientAddressAsPendingDomiciliumModel>>();
            domainQueryService = An<IDomainQueryServiceClient>();
            adUserManager = An<IADUserManager>();
            messages = SystemMessageCollection.Empty();
            clientDomiciliumGuid = combGuid.Generate();
            clientAddressKey = 321;
            clientAddressAsPendingDomicilium = new ClientAddressAsPendingDomiciliumModel(clientAddressKey, (int)AddressType.Residential);
            command = new AddClientAddressAsPendingDomiciliumCommand(clientAddressAsPendingDomicilium, clientDomiciliumGuid);
            handler = new AddClientAddressAsPendingDomiciliumCommandHandler
                (domiciliumDataManager, linkedKeyManager, domainRuleManager, combGuid, unitOfWorkFactory, eventRaiser, domainQueryService, adUserManager);
            ADUserKey = 0;
            adUserManager.WhenToldTo(x => x.GetAdUserKeyByUserName
                (serviceRequestMetaData.UserName)).Return(ADUserKey);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_retrieve_the_aduser_key = () =>
        {
            adUserManager.WasToldTo(x => x.GetAdUserKeyByUserName(serviceRequestMetaData.UserName));
        };

        private It should_return_a_message_indicating_the_user_could_not_be_found = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual("Failed to retrieve ADUserKey.");
        };

        private It should_not_add_the_domicilium = () =>
        {
            domiciliumDataManager.WasNotToldTo
                (x => x.SavePendingDomiciliumAddress(Param.IsAny<LegalEntityDomiciliumDataModel>()));
        };

        private It should_complete_the_unit_of_work = () =>
        {
            unitOfWork.WasToldTo(x => x.Complete()).OnlyOnce();
        };

        private It should_not_raise_an_event = () =>
        {
            eventRaiser.WasNotToldTo(x => x.RaiseEvent
                (Param.IsAny<DateTime>(), Param.IsAny<IEvent>(), Param.IsAny<int>(), 
                    Param.IsAny<int>(), Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}