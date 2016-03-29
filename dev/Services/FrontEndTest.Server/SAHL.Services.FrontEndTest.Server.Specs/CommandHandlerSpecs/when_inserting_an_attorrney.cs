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
    public class when_inserting_an_attorney : WithFakes
    {
        private static ITestDataManager testDataManager;
        private static IUnitOfWorkFactory uowFactory;
        private static ILinkedKeyManager linkedKeyManager;
        private static AttorneyDataModel attorney;
        private static InsertAttorneyCommand command;
        private static InsertAttorneyCommandHandler commandHandler;
        private static ISystemMessageCollection actualResult;
        private static IServiceRequestMetadata metadata;
        private static ISystemMessageCollection messages;

        private Establish context = () =>
        {
            testDataManager = An<ITestDataManager>();
            uowFactory = An<IUnitOfWorkFactory>();
            linkedKeyManager = An<ILinkedKeyManager>();
            metadata = An<IServiceRequestMetadata>();
            attorney = new AttorneyDataModel(99977, "vishavP", 15.0, 1, 20.00, 25.0, true, 1235888, true, (int)GeneralStatus.Active);
            command = new InsertAttorneyCommand(attorney, Guid.NewGuid());
            commandHandler = new InsertAttorneyCommandHandler(testDataManager, uowFactory, linkedKeyManager);
        };

        private Because of = () =>
        {
            messages = commandHandler.HandleCommand(command, metadata);
        };

        private It should_insert_the_attorney = () =>
        {
            testDataManager.WasToldTo(x => x.InsertAttorney(attorney));
        };

        private It should_return_mesagges = () =>
        {
            messages.ShouldNotBeNull();
        };

        private It should_not_return_error_messages = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}