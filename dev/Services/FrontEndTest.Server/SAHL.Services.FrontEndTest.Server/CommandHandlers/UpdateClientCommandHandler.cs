
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using SAHL.Services.FrontEndTest.Managers;

namespace SAHL.Services.FrontEndTest.CommandHandlers
{
    public class UpdateClientCommandHandler : IServiceCommandHandler<UpdateClientCommand>
    {
        private ITestDataManager testDataManager;

        public UpdateClientCommandHandler(ITestDataManager testDataManager)
        {
            this.testDataManager = testDataManager;
        }

        public ISystemMessageCollection HandleCommand(UpdateClientCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            testDataManager.UpdateClient(command.legalEntity);
            return messages;
        }
    }
}
