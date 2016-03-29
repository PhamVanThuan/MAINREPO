using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.SharedServices.Common
{
    public class CheckIsComcorpApplicationRuleCommandHandler : RuleDomainServiceCommandHandler<CheckIsComcorpApplicationRuleCommand>
    {
        private IApplicationReadOnlyRepository applicationReadOnlyRepository;

        public CheckIsComcorpApplicationRuleCommandHandler(ICommandHandler commandHandler, IApplicationReadOnlyRepository applicationReadOnlyRepository)
            : base(commandHandler)
        {
            this.applicationReadOnlyRepository = applicationReadOnlyRepository;
        }

        public override void SetupRule()
        {
            IApplication application = applicationReadOnlyRepository.GetApplicationByKey(command.ApplicationKey);
            command.RuleParameters = new object[] { application };
            base.SetupRule();
        }
    }
}