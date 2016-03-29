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
    public class when_removing_a_cats_payment_batch_item : WithFakes
    {
        private static ITestDataManager testDataManager;
        private static IServiceRequestMetadata metadata;
        private static ISystemMessageCollection messages;
        private static RemoveCATSPaymentBatchCommand command;
        private static RemoveCATSPaymentBatchCommandHandler commandHandler;
        private static int catsPaymentBatchKey;

        private Establish context = () =>
        {
            testDataManager = An<ITestDataManager>();
            catsPaymentBatchKey = 1;
            command = new RemoveCATSPaymentBatchCommand(catsPaymentBatchKey);
            commandHandler = new RemoveCATSPaymentBatchCommandHandler(testDataManager);
            metadata = An<IServiceRequestMetadata>();
        };

        private Because of = () =>
        {
            messages = commandHandler.HandleCommand(command, metadata);
        };

        private It should_remove_the_batch_item = () =>
        {
            testDataManager.WasToldTo(x => x.RemoveCATSPaymentBatch(catsPaymentBatchKey));
        };

        private It should_return_messages = () =>
        {
            messages.ShouldNotBeNull();
        };

        private It should_not_return_error_messages = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}
