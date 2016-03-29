using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using System;

namespace SAHL.Services.FrontEndTest.CommandHandlers
{
    public class InsertValuationCommandHandler : IServiceCommandHandler<InsertValuationCommand>
    {
        public ITestDataManager testDataManager { get; set; }

        public ILinkedKeyManager linkedKeyManager { get; set; }

        public IUnitOfWorkFactory uowFactory { get; set; }

        public InsertValuationCommandHandler(ITestDataManager testDataManager, ILinkedKeyManager linkedKeyManager, IUnitOfWorkFactory uowFactory)
        {
            this.testDataManager = testDataManager;
            this.linkedKeyManager = linkedKeyManager;
            this.uowFactory = uowFactory;
        }

        public Core.SystemMessages.ISystemMessageCollection HandleCommand(InsertValuationCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            using (var uow = uowFactory.Build())
            {
                testDataManager.InsertValuation(command.Valuation);
                linkedKeyManager.LinkKeyToGuid(command.Valuation.ValuationKey, command.ValuationId);
                uow.Complete();
            }
            return messages;
        }
    }
}