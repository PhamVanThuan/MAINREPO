using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Workflow.Cap2
{
    public class CheckReadvanceDoneRulesCommandHandler : RuleDomainServiceCommandHandler<CheckReadvanceDoneRulesCommand>
    {
        private ICapRepository capRepository;

        public CheckReadvanceDoneRulesCommandHandler(ICommandHandler commandHandler, ICapRepository capRepository)
            : base(commandHandler)
        {
            this.capRepository = capRepository;
        }

        public override void SetupRule()
        {
            ICapApplication capApp = capRepository.GetCapOfferByKey(command.ApplicationKey);

            command.RuleParameters = new object[] { capApp };
        }
    }
}