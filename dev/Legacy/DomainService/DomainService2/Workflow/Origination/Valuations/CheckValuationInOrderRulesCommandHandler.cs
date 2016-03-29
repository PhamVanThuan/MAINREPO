using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Workflow.Origination.Valuations
{
    public class CheckValuationInOrderRulesCommandHandler : RuleSetDomainServiceCommandHandler<CheckValuationInOrderRulesCommand>
    {
        private IPropertyRepository propertyRepository;

        public CheckValuationInOrderRulesCommandHandler(ICommandHandler commandHandler, IPropertyRepository propertyRepository)
            : base(commandHandler)
        {
            this.propertyRepository = propertyRepository;
        }

        public override void SetupRule()
        {
            IValuation valuation = propertyRepository.GetValuationByKey(command.ValuationKey);
            command.RuleParameters = new object[] { valuation };
        }
    }
}