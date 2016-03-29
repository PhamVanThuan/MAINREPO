using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Workflow.PersonalLoan
{
    public class CheckCreditSubmissionClientRulesCommandHandler : RuleSetDomainServiceCommandHandler<CheckCreditSubmissionClientRulesCommand>
    {
        IApplicationRepository applicationRepository;

        public CheckCreditSubmissionClientRulesCommandHandler(IApplicationRepository applicationRepository, ICommandHandler commandHandler)
            : base(commandHandler)
        {
            this.applicationRepository = applicationRepository;
        }

        public override void SetupRule()
        {
            IApplicationUnsecuredLending applicationUnsecuredLending = applicationRepository.GetApplicationByKey(command.ApplicationKey) as IApplicationUnsecuredLending;

            ILegalEntity legalEntity = applicationUnsecuredLending.ActiveClients[0];

            command.RuleParameters = new object[] { legalEntity };
            base.SetupRule();
        }
    }
}