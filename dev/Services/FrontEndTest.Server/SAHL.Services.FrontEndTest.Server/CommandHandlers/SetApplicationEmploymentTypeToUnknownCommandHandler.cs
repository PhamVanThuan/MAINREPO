using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using System;
using System.Linq;

namespace SAHL.Services.FrontEndTest.CommandHandlers
{
    public class SetApplicationEmploymentTypeToUnknownCommandHandler : IServiceCommandHandler<SetApplicationEmploymentTypeToUnknownCommand>
    {
        private ITestDataManager testDataManager;

        public SetApplicationEmploymentTypeToUnknownCommandHandler(ITestDataManager testDataManager)
        {
            this.testDataManager = testDataManager;
        }

        public ISystemMessageCollection HandleCommand(SetApplicationEmploymentTypeToUnknownCommand command, IServiceRequestMetadata metadata)
        {
            var systemMessages = SystemMessageCollection.Empty();
            this.testDataManager.UpdateApplicationEmploymentType(command.ApplicationNumber, EmploymentType.Unknown);
            return systemMessages;
        }
    }
}