using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;

namespace SAHL.Services.FrontEndTest.CommandHandlers
{
    public class RemoveCATSPaymentBatchCommandHandler : IServiceCommandHandler<RemoveCATSPaymentBatchCommand>
    {
        private ITestDataManager testDataManager;

        public RemoveCATSPaymentBatchCommandHandler(ITestDataManager testDataManager)
        {
            this.testDataManager = testDataManager;
        }

        public ISystemMessageCollection HandleCommand(RemoveCATSPaymentBatchCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            this.testDataManager.RemoveCATSPaymentBatch(command.CATSPaymentBatchKey);
            return messages;
        }
    }
}