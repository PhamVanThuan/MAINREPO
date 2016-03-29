using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Workflow.DebtCounselling
{
    public class CheckTenDaysTerminationReminderRulesCommandHandler : RuleSetDomainServiceCommandHandler<CheckTenDaysTerminationReminderRulesCommand>
    {
        private IDebtCounsellingRepository debtCounsellingRepository;

        public CheckTenDaysTerminationReminderRulesCommandHandler(ICommandHandler commandHandler, IDebtCounsellingRepository debtCounsellingRepository)
            : base(commandHandler)
        {
            this.debtCounsellingRepository = debtCounsellingRepository;
        }

        public override void SetupRule()
        {
            var debtCounselling = debtCounsellingRepository.GetDebtCounsellingByKey(command.DebtCounsellingKey);

            command.RuleParameters = new object[] { debtCounselling };
        }
    }
}