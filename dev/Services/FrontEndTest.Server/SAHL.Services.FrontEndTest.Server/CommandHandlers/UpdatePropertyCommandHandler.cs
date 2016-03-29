using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;

namespace SAHL.Services.FrontEndTest.CommandHandlers
{
    public class UpdatePropertyCommandHandler : IServiceCommandHandler<UpdatePropertyCommand>
    {
        private ITestDataManager testDataManager;

        public UpdatePropertyCommandHandler(ITestDataManager testDataManager)
        {
            this.testDataManager = testDataManager;
        }
        public ISystemMessageCollection HandleCommand(UpdatePropertyCommand command, IServiceRequestMetadata metadata)
        {
            var systemMessages = SystemMessageCollection.Empty();
            this.testDataManager.UpdateProperty(command.Property);
            return systemMessages;
        }
    }
}
