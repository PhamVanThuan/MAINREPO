using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;

namespace SAHL.Services.FrontEndTest.CommandHandlers
{
    public class RemoveValuerCommandHandler : IServiceCommandHandler<RemoveValuerCommand>
    {
        public ITestDataManager testDataManager { get; set; }

        public RemoveValuerCommandHandler(ITestDataManager testDataManager)
        {
            this.testDataManager = testDataManager;
        }

        public ISystemMessageCollection HandleCommand(RemoveValuerCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            testDataManager.RemoveValuer(command.ValuatorKey);
            return messages;
        }
    }
}