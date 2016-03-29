using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Workflow.PersonalLoan
{
    public class CheckCreditSubmissionRulesCommandHandler : RuleSetDomainServiceCommandHandler<CheckCreditSubmissionRulesCommand>
    {
        IApplicationRepository applicationRepository;

        public CheckCreditSubmissionRulesCommandHandler(IApplicationRepository applicationRepository, ICommandHandler commandHandler)
            : base(commandHandler)
        {
            this.applicationRepository = applicationRepository;
        }

        public override void SetupRule()
        {
            IApplication application = applicationRepository.GetApplicationByKey(command.ApplicationKey);
            command.RuleParameters = new object[] { application };
            base.SetupRule();
        }
    }
}