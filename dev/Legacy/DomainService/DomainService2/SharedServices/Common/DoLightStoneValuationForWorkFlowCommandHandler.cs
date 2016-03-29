using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.SharedServices.Common
{
    public class DoLightStoneValuationForWorkFlowCommandHandler : IHandlesDomainServiceCommand<DoLightStoneValuationForWorkFlowCommand>
    {
        IPropertyRepository propertyRepository;

        public DoLightStoneValuationForWorkFlowCommandHandler(IPropertyRepository propertyRepository)
        {
            this.propertyRepository = propertyRepository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, DoLightStoneValuationForWorkFlowCommand command)
        {
            propertyRepository.GetLightStoneValuationForWorkFlow(command.ApplicationKey, command.ADUser);
        }
    }
}