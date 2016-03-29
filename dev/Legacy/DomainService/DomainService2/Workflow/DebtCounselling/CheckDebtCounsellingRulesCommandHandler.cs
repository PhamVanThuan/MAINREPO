using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Workflow.DebtCounselling
{
    public class CheckDebtCounsellingRulesCommandHandler : IHandlesDomainServiceCommand<CheckDebtCounsellingRulesCommand>
    {
        private IDebtCounsellingRepository debtCounsellingRepository;

        public CheckDebtCounsellingRulesCommandHandler(IDebtCounsellingRepository debtCounsellingRepository)
        {
            this.debtCounsellingRepository = debtCounsellingRepository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, CheckDebtCounsellingRulesCommand command)
        {
            var debtCounselling = debtCounsellingRepository.GetDebtCounsellingByKey(command.DebtCounsellingKey);

            command.RuleParameters = new object[] { debtCounselling };
        }
    }
}