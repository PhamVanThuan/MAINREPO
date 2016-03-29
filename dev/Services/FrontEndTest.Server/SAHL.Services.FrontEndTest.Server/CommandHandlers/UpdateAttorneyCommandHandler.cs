using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;

namespace SAHL.Services.FrontEndTest.CommandHandlers
{
    public class UpdateAttorneyCommandHandler : IServiceCommandHandler<UpdateAttorneyCommand>
    {
        
        public ITestDataManager testDataManager { get; set; }
        public UpdateAttorneyCommandHandler(ITestDataManager testDataManager)
        {
            this.testDataManager = testDataManager;
        }
        public ISystemMessageCollection HandleCommand(UpdateAttorneyCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            testDataManager.UpdateAttorney(command.Attorney);
            return messages;
        }
    }
}
