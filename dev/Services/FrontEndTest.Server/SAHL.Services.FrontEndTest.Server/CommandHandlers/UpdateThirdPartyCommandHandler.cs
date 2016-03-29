using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using SAHL.Services.FrontEndTest.Managers;

namespace SAHL.Services.FrontEndTest.CommandHandlers
{
    public class UpdateThirdPartyCommandHandler : IServiceCommandHandler<UpdateThirdPartyCommand>
    {
        private ITestDataManager testDataManager;

        public UpdateThirdPartyCommandHandler(ITestDataManager testDataManager)
        {
            this.testDataManager = testDataManager;
        }

        public ISystemMessageCollection HandleCommand(UpdateThirdPartyCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            testDataManager.UpdateThirdParty(command.thirdparty);
            return messages;
        }
    }
}
