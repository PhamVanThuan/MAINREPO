using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;

namespace SAHL.Services.FrontEndTest.CommandHandlers
{
    public class InsertValuerCommandHandler : IServiceCommandHandler<InsertValuerCommand>
    {
        private ITestDataManager testDataManager;
        private IUnitOfWorkFactory uowFactory;
        private ILinkedKeyManager linkedKeyManager;

        public InsertValuerCommandHandler(ITestDataManager testDataManager, IUnitOfWorkFactory uowFactory, ILinkedKeyManager linkedKeyManager)
        {
            this.testDataManager = testDataManager;
            this.uowFactory = uowFactory;
            this.linkedKeyManager = linkedKeyManager;
        }

        public ISystemMessageCollection HandleCommand(InsertValuerCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            using (var uow = uowFactory.Build())
            {
                testDataManager.InsertValuer(command.Valuer);
                linkedKeyManager.LinkKeyToGuid(command.Valuer.ValuatorKey, command.ValuerId);
                uow.Complete();
            }
            return messages;
        }
    }
}