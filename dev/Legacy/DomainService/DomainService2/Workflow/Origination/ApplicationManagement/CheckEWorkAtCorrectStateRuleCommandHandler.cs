using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class CheckEWorkAtCorrectStateRuleCommandHandler : RuleDomainServiceCommandHandler<CheckEWorkAtCorrectStateRuleCommand>
    {
        protected IApplicationRepository ApplicationRepository;

        public CheckEWorkAtCorrectStateRuleCommandHandler(ICommandHandler commandHandler, IApplicationRepository apprepo)
            : base(commandHandler)
        {
            this.ApplicationRepository = apprepo;
        }

        public override void SetupRule()
        {
            IApplication app = ApplicationRepository.GetApplicationByKey(command.ApplicationKey);
            command.RuleParameters = new object[] { app };
        }
    }
}