using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Workflow.Origination.ApplicationCapture
{
    public class CheckBranchSubmitApplicationRulesCommandHandler : RuleSetDomainServiceCommandHandler<CheckBranchSubmitApplicationRulesCommand>
    {
        private IApplicationRepository applicationRepository;

        public CheckBranchSubmitApplicationRulesCommandHandler(ICommandHandler commandHandler, IApplicationRepository applicationRepository)
            : base(commandHandler)
        {
            this.applicationRepository = applicationRepository;
        }

        public override void SetupRule()
        {
            var application = applicationRepository.GetApplicationByKey(command.ApplicationKey);
            command.RuleParameters = new object[] { application };
        }
    }
}