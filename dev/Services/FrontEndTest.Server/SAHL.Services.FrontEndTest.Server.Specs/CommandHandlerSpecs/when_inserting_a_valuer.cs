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
    public class when_inserting_a_valuer : WithFakes
    {
        private static ITestDataManager testDataManager;
        private static IUnitOfWorkFactory uowFactory;
        private static ILinkedKeyManager linkedKeyManager;
        private static ISystemMessageCollection messages;
        private static IServiceRequestMetadata metadata;
        private static InsertValuerCommand command;
        private static InsertValuerCommandHandler commandHandler;
        private static ValuatorDataModel valuator;

        private Establish context = () =>
            {
                testDataManager = An<ITestDataManager>();
                uowFactory = An<IUnitOfWorkFactory>();
                linkedKeyManager = An<ILinkedKeyManager>();
                metadata = An<IServiceRequestMetadata>();
                valuator = new ValuatorDataModel("valuatorContact", "valuatorPassword", 1, (int)GeneralStatus.Active, 1235888);
                command = new InsertValuerCommand(valuator,Guid.NewGuid());
                commandHandler = new InsertValuerCommandHandler(testDataManager, uowFactory, linkedKeyManager);
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

        private It should_insert_the_valuer = () =>
        {
            testDataManager.WasToldTo(x=>x.InsertValuer(valuator));
        };

        private It should_link_the_guid_to_the_key = () =>
        {
            linkedKeyManager.WasToldTo(x => x.LinkKeyToGuid(command.Valuer.ValuatorKey, command.ValuerId));
        };
    }
}
