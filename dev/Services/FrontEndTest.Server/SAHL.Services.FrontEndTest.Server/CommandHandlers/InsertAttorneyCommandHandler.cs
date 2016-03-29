using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;

namespace SAHL.Services.FrontEndTest.CommandHandlers
{
    public class InsertAttorneyCommandHandler : IServiceCommandHandler<InsertAttorneyCommand>
    {
        private ITestDataManager testDataManager;
        private IUnitOfWorkFactory uowFactory;
        private ILinkedKeyManager linkedKeyManager;

        public InsertAttorneyCommandHandler(ITestDataManager testDataManager, IUnitOfWorkFactory uowFactory, ILinkedKeyManager linkedKeyManager)
        {
            this.testDataManager = testDataManager;
            this.uowFactory = uowFactory;
            this.linkedKeyManager = linkedKeyManager;
        }

        public Core.SystemMessages.ISystemMessageCollection HandleCommand(InsertAttorneyCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            using (var uow = uowFactory.Build())
            {
                testDataManager.InsertAttorney(command.Attorney);
                linkedKeyManager.LinkKeyToGuid(command.Attorney.AttorneyKey, command.AttorneyId);
                uow.Complete();
            }
            return messages;
        }
    }
}