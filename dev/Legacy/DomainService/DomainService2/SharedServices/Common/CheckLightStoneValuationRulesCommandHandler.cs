using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.SharedServices.Common
{
    public class CheckLightStoneValuationRulesCommandHandler : RuleSetDomainServiceCommandHandler<CheckLightStoneValuationRulesCommand>
    {
        IPropertyRepository propertyRepository;

        public CheckLightStoneValuationRulesCommandHandler(ICommandHandler commandHandler, IPropertyRepository propertyRepository)
            : base(commandHandler)
        {
            this.propertyRepository = propertyRepository;
        }

        public override void SetupRule()
        {
            IProperty property = propertyRepository.GetPropertyByApplicationKey(command.ApplicationKey);
            command.RuleParameters = new object[] { property };
        }
    }
}