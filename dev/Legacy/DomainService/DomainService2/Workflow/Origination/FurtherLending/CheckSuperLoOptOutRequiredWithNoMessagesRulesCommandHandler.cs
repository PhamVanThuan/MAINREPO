using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Workflow.Origination.FurtherLending
{
    public class CheckSuperLoOptOutRequiredWithNoMessagesRulesCommandHandler : RuleSetDomainServiceCommandHandler<CheckSuperLoOptOutRequiredWithNoMessagesRulesCommand>
    {
        protected IApplicationRepository ApplicationRepository;

        public CheckSuperLoOptOutRequiredWithNoMessagesRulesCommandHandler(ICommandHandler commandHandler, IApplicationRepository applicationRepository)
            : base(commandHandler, true, true)
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