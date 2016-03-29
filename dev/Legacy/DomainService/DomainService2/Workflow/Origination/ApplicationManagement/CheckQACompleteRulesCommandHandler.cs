using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class CheckQACompleteRulesCommandHandler : RuleSetDomainServiceCommandHandler<CheckQACompleteRulesCommand>
    {
        IApplicationRepository applicationRepository;

        public CheckQACompleteRulesCommandHandler(ICommandHandler commandHandler, IApplicationRepository applicationRepository)
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