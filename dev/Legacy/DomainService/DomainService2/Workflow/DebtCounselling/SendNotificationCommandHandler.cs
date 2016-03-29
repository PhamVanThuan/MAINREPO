using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DomainService2.Workflow.DebtCounselling
{
    public class SendNotificationCommandHandler : IHandlesDomainServiceCommand<SendNotificationCommand>
    {
        protected IDebtCounsellingRepository debtCounsellingRepository;
        public SendNotificationCommandHandler(IDebtCounsellingRepository debtCounsellingRepository)
        {
            this.debtCounsellingRepository = debtCounsellingRepository;
        }
        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, SendNotificationCommand command)
        {
            IDebtCounselling debtCounselling = debtCounsellingRepository.GetDebtCounsellingByKey(command.DebtCounsellingKey);
            if (debtCounselling == null)
                throw new Exception("No DebtCounselling record exists.");

            debtCounsellingRepository.SendNotification(debtCounselling);
        }
    }
}
