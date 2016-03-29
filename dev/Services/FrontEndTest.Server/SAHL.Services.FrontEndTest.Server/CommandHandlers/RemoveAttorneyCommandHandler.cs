using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;

namespace SAHL.Services.FrontEndTest.CommandHandlers
{
    public class RemoveAttorneyCommandHandler : IServiceCommandHandler<RemoveAttorneyCommand>
    {
        public ITestDataManager testDataManager { get; set; }

        public RemoveAttorneyCommandHandler(ITestDataManager testDataManager)
        {
            this.testDataManager = testDataManager;
        }

        public ISystemMessageCollection HandleCommand(RemoveAttorneyCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            testDataManager.RemoveAttorney(command.AttorneyKey);
            return messages;
        }
    }
}