using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;

namespace SAHL.Services.FrontEndTest.CommandHandlers
{
    public class InsertDisabilityClaimCommandHandler : IServiceCommandHandler<InsertDisabilityClaimCommand>
    {
        private ITestDataManager testDataManager;
        private ILinkedKeyManager linkedKeyManager;
        private IUnitOfWorkFactory uowFactory;

        public InsertDisabilityClaimCommandHandler(ITestDataManager testDataManager, ILinkedKeyManager linkedKeyManager, IUnitOfWorkFactory uowFactory)
        {
            this.testDataManager = testDataManager;
            this.linkedKeyManager = linkedKeyManager;
            this.uowFactory = uowFactory;
        }

        public ISystemMessageCollection HandleCommand(InsertDisabilityClaimCommand command, IServiceRequestMetadata metadata)
        {
            var systemMessages = SystemMessageCollection.Empty();
            using (var uow = uowFactory.Build())
            {              
                this.testDataManager.InsertDisabilityClaimRecord(command.model);
                this.linkedKeyManager.LinkKeyToGuid(command.model.DisabilityClaimKey, command.disabilityClaimGuid);
                if(command.model.DisabilityClaimStatusKey == (int)DisabilityClaimStatus.Approved)
                {
                    var createPaymentScheduleResult  = this.testDataManager.CreateDisabilityClaimPaymentSchedule(command.model.DisabilityClaimKey, metadata.UserName);                

                    if (!string.IsNullOrEmpty(createPaymentScheduleResult))
                    {
                        systemMessages.AddMessage(new SystemMessage(createPaymentScheduleResult, SystemMessageSeverityEnum.Error));
                        return systemMessages;
                    }
                }
                uow.Complete();
            }
            return systemMessages;
        }
    }
}