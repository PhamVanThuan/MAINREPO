using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using System;
using System.Linq;

namespace SAHL.Services.FrontEndTest.CommandHandlers
{
    public class RemoveApplicantFromActiveNewBusinessApplicantsCommandHandler : IServiceCommandHandler<RemoveApplicantFromActiveNewBusinessApplicantsCommand>
    {
        private ITestDataManager testDataManager;

        public RemoveApplicantFromActiveNewBusinessApplicantsCommandHandler(ITestDataManager testDataManager)
        {
            this.testDataManager = testDataManager;
        }

        public ISystemMessageCollection HandleCommand(RemoveApplicantFromActiveNewBusinessApplicantsCommand command, IServiceRequestMetadata metadata)
        {
            this.testDataManager.RemoveActiveNewBusinessApplicantRecord(command.OfferRoleKey);
            return SystemMessageCollection.Empty();
        }
    }
}