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
    public class when_inserting_a_disability_claim : WithFakes
    {
        private static ITestDataManager testDataManager;
        private static ILinkedKeyManager linkedKeyManager;
        private static IUnitOfWorkFactory uowFactory;
        private static ISystemMessageCollection messages;
        private static IServiceRequestMetadata metadata;
        private static DisabilityClaimDataModel model;
        private static InsertDisabilityClaimCommand command;
        private static InsertDisabilityClaimCommandHandler commandHandler;
        private static int disabilityClaimKey;

        private Establish context = () =>
        {
            testDataManager = An<ITestDataManager>();
            uowFactory = An<IUnitOfWorkFactory>();
            linkedKeyManager = An<ILinkedKeyManager>();
            metadata = An<IServiceRequestMetadata>();
            model = new DisabilityClaimDataModel(1, 1, DateTime.Now, DateTime.Now, DateTime.Now, "", 1, "", DateTime.Now, (int)DisabilityClaimStatus.Approved,
                                                 DateTime.Now, 1, DateTime.Today);
            command = new InsertDisabilityClaimCommand(model, Guid.NewGuid());
            commandHandler = new InsertDisabilityClaimCommandHandler(testDataManager, linkedKeyManager, uowFactory);
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

        private It should_insert_the_disability_claim = () =>
            {
                testDataManager.WasToldTo(x => x.InsertDisabilityClaimRecord(model));
            };
    }
}