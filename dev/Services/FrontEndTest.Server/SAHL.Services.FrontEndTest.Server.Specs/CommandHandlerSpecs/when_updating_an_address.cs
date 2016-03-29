using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.CommandHandlers;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using System;

namespace SAHL.Services.FrontEndTest.Server.Specs.CommandHandlerSpecs
{
    internal class when_updating_an_address : WithFakes
    {
        private static ITestDataManager testDataManager;
        private static UpdateAddressCommand command;
        private static AddressDataModel address;
        private static int legalEntityKey;
        private static UpdateAddressCommandHandler commandHandler;
        private static IServiceRequestMetadata metadata;
        private static ISystemMessageCollection messages;

        private Establish context = () =>
        {
            legalEntityKey = 1235888;
            address = new AddressDataModel(123456789, "BoxNumber", "UnitNumber", "BuildingNumber", "BuildingName", "streetNumber",
                                           "streetName", 999, 4051, "RSA", "KZN", "Durban", "Avoca", "4051", "VishavP", DateTime.Now,
                                           "suteNumber", "ft1", "ft2", "ft3", "ft4", "ft5");
            command = new UpdateAddressCommand(address);
            testDataManager = An<ITestDataManager>();
            commandHandler = new UpdateAddressCommandHandler(testDataManager);
            metadata = An<IServiceRequestMetadata>();
        };

        private Because of = () =>
        {
            messages = commandHandler.HandleCommand(command, metadata);
        };

        private It should_use_the_update_statement_through_the_testDataManager_to_update_the_variableLoanInfo = () =>
        {
            testDataManager.WasToldTo(x => x.UpdateAddress(address));
        };

        private It should_return_messages = () =>
        {
            messages.ShouldNotBeNull();
        };

        private It should_not_return_errors = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}