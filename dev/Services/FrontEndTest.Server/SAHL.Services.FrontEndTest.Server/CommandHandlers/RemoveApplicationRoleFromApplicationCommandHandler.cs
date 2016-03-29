using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using System;
using System.Linq;

namespace SAHL.Services.FrontEndTest.CommandHandlers
{
    public class RemoveApplicationRoleFromApplicationCommandHandler : IServiceCommandHandler<RemoveApplicationRoleFromApplicationCommand>
    {
        private ITestDataManager testDataManager;

        public RemoveApplicationRoleFromApplicationCommandHandler(ITestDataManager testDataManager)
        {
            this.testDataManager = testDataManager;
        }

        public Core.SystemMessages.ISystemMessageCollection HandleCommand(RemoveApplicationRoleFromApplicationCommand command, IServiceRequestMetadata metadata)
        {
            this.testDataManager.RemoveApplicationRole(command.OfferRoleKey);
            return SystemMessageCollection.Empty();
        }
    }
}