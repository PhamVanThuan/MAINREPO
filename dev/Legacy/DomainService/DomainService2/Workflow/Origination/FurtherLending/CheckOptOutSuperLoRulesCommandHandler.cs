using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Workflow.Origination.FurtherLending
{
    public class CheckOptOutSuperLoRulesCommandHandler : RuleSetDomainServiceCommandHandler<CheckOptOutSuperLoRulesCommand>
    {
        protected IApplicationRepository ApplicationRepository;

        public CheckOptOutSuperLoRulesCommandHandler(ICommandHandler commandHandler, IApplicationRepository applicationRepository)
            : base(commandHandler)
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