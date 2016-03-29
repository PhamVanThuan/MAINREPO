using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Workflow.Origination.FurtherLending
{
    public class CheckIsFurtherAdvanceBelowLAARulesCommandHandler : RuleSetDomainServiceCommandHandler<CheckIsFurtherAdvanceBelowLAARulesCommand>
    {
        protected IApplicationRepository ApplicationRepository;

        public CheckIsFurtherAdvanceBelowLAARulesCommandHandler(ICommandHandler commandHandler, IApplicationRepository applicationRepository)
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