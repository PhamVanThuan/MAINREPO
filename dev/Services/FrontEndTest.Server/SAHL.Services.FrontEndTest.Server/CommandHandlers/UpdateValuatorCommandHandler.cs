using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;

namespace SAHL.Services.FrontEndTest.CommandHandlers
{
    public class UpdateValuatorCommandHandler : IServiceCommandHandler<UpdateValuatorCommand>
    {
        
        public ITestDataManager testDataManager { get; set; }
        public UpdateValuatorCommandHandler(ITestDataManager testDataManager)
        {
            this.testDataManager = testDataManager;
        }
        public ISystemMessageCollection HandleCommand(UpdateValuatorCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            testDataManager.UpdateValuator(command.Valuator);
            return messages;
        }
    }
}
