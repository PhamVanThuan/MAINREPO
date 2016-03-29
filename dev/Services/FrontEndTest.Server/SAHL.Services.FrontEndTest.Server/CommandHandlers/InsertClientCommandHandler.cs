using System;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.BusinessModel.Enums;

namespace SAHL.Services.FrontEndTest.CommandHandlers
{
    public class InsertClientCommandHandler : IServiceCommandHandler<InsertClientCommand>
    {
        private ITestDataManager testDataManager;
        private IUnitOfWorkFactory uowFactory;
        private ILinkedKeyManager linkedKeyManager;

        public InsertClientCommandHandler(ITestDataManager testDataManager, IUnitOfWorkFactory uowFactory, ILinkedKeyManager linkedKeyManager)
        {
            this.testDataManager = testDataManager;
            this.uowFactory = uowFactory;
            this.linkedKeyManager = linkedKeyManager;
        }

        public ISystemMessageCollection HandleCommand(InsertClientCommand command, IServiceRequestMetadata metadata)
        {
            var systemMessages = SystemMessageCollection.Empty();
            using (var uow = uowFactory.Build())
            {
                this.testDataManager.InsertClient(command.LegalEntity);
                var OfferRole = new OfferRoleDataModel(command.LegalEntity.LegalEntityKey, command.OfferKey,(int)OfferRoleType.MainApplicant,(int)GeneralStatus.Active,DateTime.Now);
                this.testDataManager.LinkClientToOffer(OfferRole);
                linkedKeyManager.LinkKeyToGuid(OfferRole.OfferRoleKey, command.ClientGuid);
                uow.Complete();
            }
            return systemMessages;
        }
    }
}
