using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.CommandHandlers;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using System;

namespace SAHL.Services.FrontEndTest.Server.Specs.CommandHandlerSpecs
{
    public class when_linking_a_legalEntity_to_an_address : WithFakes
    {
        private static ITestDataManager testDataManager;
        private static IUnitOfWorkFactory uowFactory;
        private static ILinkedKeyManager linkedKeyManager;
        private static ISystemMessageCollection messages;
        private static IServiceRequestMetadata metadata;
        private static LinkLegalEntityAddressCommand command;
        private static LinkLegalEntityAddressCommandHandler commandHandler;
        private static LegalEntityAddressDataModel model;
        private static int addressKey;
        private static AddressType AddressType;
        private static int legalEntityKey;
        private static int addressTypeKey;

        private Establish context = () =>
        {
            testDataManager = An<ITestDataManager>();
            metadata = An<IServiceRequestMetadata>();
            AddressType = AddressType.Postal;
            addressTypeKey = 1;
            addressKey = 1;
            legalEntityKey = 1;
            command = new LinkLegalEntityAddressCommand(legalEntityKey,addressKey,(AddressType)addressTypeKey);
            commandHandler = new LinkLegalEntityAddressCommandHandler(testDataManager);
            model = new LegalEntityAddressDataModel(command.LegalEntityKey, command.AddressKey, addressTypeKey, DateTime.Now, (int)GeneralStatus.Pending);
        };

        private Because of = () =>
        {
            messages = commandHandler.HandleCommand(command, metadata);
        };

        private It should_return_messages = () =>
        {
            messages.ShouldNotBeNull();
        };

        private It should_not_return_error_messages = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };

        private It should_link_the_legal_entity_address = () =>
        {
            //testDataManager.WasToldTo(x => x.InsertLegalEntityAddress(model));
        }; 
            
    }
}
