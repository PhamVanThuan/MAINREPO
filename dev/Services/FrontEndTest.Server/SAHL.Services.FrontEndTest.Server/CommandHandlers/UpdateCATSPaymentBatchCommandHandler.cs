using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;

namespace SAHL.Services.FrontEndTest.CommandHandlers
{
    public class UpdateCATSPaymentBatchCommandHandler : IServiceCommandHandler<UpdateCATSPaymentBatchCommand>
    {
        private ITestDataManager testDataManager;

        public UpdateCATSPaymentBatchCommandHandler(ITestDataManager testDataManager)
        {
            this.testDataManager = testDataManager;
        }

        public Core.SystemMessages.ISystemMessageCollection HandleCommand(UpdateCATSPaymentBatchCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            testDataManager.UpdateCATSPaymentBatch(command.CATSPaymentBatch);
            return messages;
        }
    }
}