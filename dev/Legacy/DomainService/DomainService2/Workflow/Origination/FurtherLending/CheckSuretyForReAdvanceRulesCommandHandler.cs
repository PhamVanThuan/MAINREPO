using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Workflow.Origination.FurtherLending
{
    public class CheckSuretyForReAdvanceRulesCommandHandler : RuleSetDomainServiceCommandHandler<CheckSuretyForReAdvanceRulesCommand>
    {
        protected IApplicationRepository ApplicationRepository;

        public CheckSuretyForReAdvanceRulesCommandHandler(ICommandHandler commandHandler, IApplicationRepository applicationRepository)
            : base(commandHandler, true)
        {
            this.ApplicationRepository = applicationRepository;
        }

        public override void SetupRule()
        {
            IApplication application = ApplicationRepository.GetApplicationByKey(command.ApplicationKey);
            command.RuleParameters = new object[] { application };
        }
    }
}