using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Workflow.PersonalLoan
{
    public class CheckSendOfferRulesCommandHandler : RuleSetDomainServiceCommandHandler<CheckSendOfferRulesCommand>
    {
        IApplicationRepository applicationRepository;

        public CheckSendOfferRulesCommandHandler(IApplicationRepository applicationRepository, ICommandHandler commandHandler)
            : base(commandHandler)
        {
            this.applicationRepository = applicationRepository;
        }

        public override void SetupRule()
        {
            IApplication application = applicationRepository.GetApplicationByKey(command.ApplicationKey);
            command.RuleParameters = new object[] { application };
        }
    }
}