using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;

namespace SAHL.Services.FrontEndTest.CommandHandlers
{
    public class RemoveApplicationMailingAddressCommandHandler : IServiceCommandHandler<RemoveApplicationMailingAddressCommand>
    {
        private ITestDataManager testDataManager;

        public RemoveApplicationMailingAddressCommandHandler(ITestDataManager testDataManager)
        {
            this.testDataManager = testDataManager;
        }

        public ISystemMessageCollection HandleCommand(RemoveApplicationMailingAddressCommand command, IServiceRequestMetadata metadata)
        {
            this.testDataManager.RemoveApplicationMailingAddress(command.ApplicationNumber);
            return SystemMessageCollection.Empty();
        }
    }
}