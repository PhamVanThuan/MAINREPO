using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using System;
using System.Linq;

namespace SAHL.Services.FrontEndTest.CommandHandlers
{
    public class SetApplicationSPVToZeroCommandHandler : IServiceCommandHandler<SetApplicationSPVToZeroCommand>
    {
        private ITestDataManager testDataManager;

        public SetApplicationSPVToZeroCommandHandler(ITestDataManager testDataManager)
        {
            this.testDataManager = testDataManager;
        }

        public ISystemMessageCollection HandleCommand(SetApplicationSPVToZeroCommand command, IServiceRequestMetadata metadata)
        {
            this.testDataManager.SetOfferInformationSPV(command.ApplicationInformationKey, 0);
            return SystemMessageCollection.Empty();
        }
    }
}