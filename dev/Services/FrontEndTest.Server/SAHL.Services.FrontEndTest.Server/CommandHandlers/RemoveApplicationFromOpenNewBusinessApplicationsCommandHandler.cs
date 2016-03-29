using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using System;
using System.Linq;

namespace SAHL.Services.FrontEndTest.CommandHandlers
{
    public class RemoveApplicationFromOpenNewBusinessApplicationsCommandHandler : IServiceCommandHandler<RemoveApplicationFromOpenNewBusinessApplicationsCommand>
    {
        private ITestDataManager testDataManager;

        public RemoveApplicationFromOpenNewBusinessApplicationsCommandHandler(ITestDataManager testDataManager)
        {
            this.testDataManager = testDataManager;
        }

        public ISystemMessageCollection HandleCommand(RemoveApplicationFromOpenNewBusinessApplicationsCommand command, IServiceRequestMetadata metadata)
        {
            this.testDataManager.RemoveOpenNewBusinessApplicationCommand(command.ApplicationNumber);
            return SystemMessageCollection.Empty();
        }
    }
}