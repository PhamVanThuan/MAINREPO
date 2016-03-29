using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;

namespace SAHL.Services.FrontEndTest.CommandHandlers
{
    public class SetHouseholdIncomeToZeroCommandHandler : IServiceCommandHandler<SetHouseholdIncomeToZeroCommand>
    {
        private ITestDataManager testDataManager;

        public SetHouseholdIncomeToZeroCommandHandler(ITestDataManager testDataManager)
        {
            this.testDataManager = testDataManager;
        }
        public ISystemMessageCollection HandleCommand(SetHouseholdIncomeToZeroCommand command, IServiceRequestMetadata metadata)
        {
            var systemMessages = SystemMessageCollection.Empty();
            this.testDataManager.UpdateHouseholdIncomeToZero(command.ApplicationNumber);
            return systemMessages;
        }
    }
}