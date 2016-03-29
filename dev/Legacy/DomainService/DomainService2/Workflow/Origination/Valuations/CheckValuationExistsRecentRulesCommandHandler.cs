using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Workflow.Origination.Valuations
{
    public class CheckValuationExistsRecentRulesCommandHandler : RuleSetDomainServiceCommandHandler<CheckValuationExistsRecentRulesCommand>
    {
        private IApplicationRepository applicationRepository;

        public CheckValuationExistsRecentRulesCommandHandler(ICommandHandler commandHandler, IApplicationRepository applicationRepository)
            : base(commandHandler)
        {
            this.applicationRepository = applicationRepository;
        }

        public override void SetupRule()
        {
            IApplicationMortgageLoan applicationMortgageLoan = applicationRepository.GetApplicationByKey(command.ApplicationKey) as IApplicationMortgageLoan;
            command.RuleParameters = new object[] { applicationMortgageLoan };
        }
    }
}