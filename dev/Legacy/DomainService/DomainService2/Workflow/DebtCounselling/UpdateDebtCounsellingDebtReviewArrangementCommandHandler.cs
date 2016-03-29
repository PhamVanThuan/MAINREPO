using System;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;

namespace DomainService2.Workflow.DebtCounselling
{
    public class UpdateDebtCounsellingDebtReviewArrangementCommandHandler : IHandlesDomainServiceCommand<UpdateDebtCounsellingDebtReviewArrangementCommand>
    {
        private IDebtCounsellingRepository debtcounsellingRepository;
        private ICommonRepository commonRepository;

        public UpdateDebtCounsellingDebtReviewArrangementCommandHandler(IDebtCounsellingRepository debtcounsellingRepository, ICommonRepository commonRepository)
        {
            this.debtcounsellingRepository = debtcounsellingRepository;
            this.commonRepository = commonRepository;
        }

        public void Handle(IDomainMessageCollection messages, UpdateDebtCounsellingDebtReviewArrangementCommand command)
        {
            command.Result = debtcounsellingRepository.UpdateDebtCounsellingDebtReviewArrangement(command.AccountKey, command.UserID);
            commonRepository.RefreshDAOObject<IAccount>(command.AccountKey);
        }
    }
}