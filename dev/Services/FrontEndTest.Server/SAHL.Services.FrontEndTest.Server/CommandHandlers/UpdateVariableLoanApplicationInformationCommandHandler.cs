using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;

namespace SAHL.Services.FrontEndTest.CommandHandlers
{
    public class UpdateVariableLoanApplicationInformationCommandHandler : IServiceCommandHandler<UpdateVariableLoanApplicationInformationCommand>
    {
        private ITestDataManager testDataManager;

        public UpdateVariableLoanApplicationInformationCommandHandler(ITestDataManager testDataManager)
        {
            this.testDataManager = testDataManager;
        }

        public ISystemMessageCollection HandleCommand(UpdateVariableLoanApplicationInformationCommand command, IServiceRequestMetadata metadata)
        {
            var systemMessages = SystemMessageCollection.Empty();
            this.testDataManager.UpdateVariableLoanApplicationInformationStatement(command.VariableLoanApplicationInformation);
            return systemMessages;
        }
    }
}