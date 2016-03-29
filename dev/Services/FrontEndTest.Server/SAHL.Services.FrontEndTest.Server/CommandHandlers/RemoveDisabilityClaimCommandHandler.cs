using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;

namespace SAHL.Services.FrontEndTest.CommandHandlers
{
    public class RemoveDisabilityClaimCommandHandler : IServiceCommandHandler<RemoveDisabilityClaimCommand>
    {
        private ITestDataManager testDataManager;

        public RemoveDisabilityClaimCommandHandler(ITestDataManager testDataManager)
        {
            this.testDataManager = testDataManager;
        }

        public ISystemMessageCollection HandleCommand(RemoveDisabilityClaimCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            this.testDataManager.RemoveDisabilityPaymentRecord(command.DisabilityClaimKey);
            this.testDataManager.RemoveDisabilityClaimRecord(command.DisabilityClaimKey);
            return messages;
        }
    }
}
