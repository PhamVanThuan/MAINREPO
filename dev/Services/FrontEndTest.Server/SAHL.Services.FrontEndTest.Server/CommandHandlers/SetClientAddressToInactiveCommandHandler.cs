using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;

namespace SAHL.Services.FrontEndTest.CommandHandlers
{
    public class SetClientAddressToInactiveCommandHandler : IServiceCommandHandler<SetClientAddressToInactiveCommand>
    {
        private ITestDataManager testDataManager;

        public SetClientAddressToInactiveCommandHandler(ITestDataManager testDataManager)
        {
            this.testDataManager = testDataManager;
        }

        public ISystemMessageCollection HandleCommand(SetClientAddressToInactiveCommand command, IServiceRequestMetadata metadata)
        {
            this.testDataManager.SetClientAddressToInactive(command.ClientAddressKey);
            return SystemMessageCollection.Empty();
        }
    }
}