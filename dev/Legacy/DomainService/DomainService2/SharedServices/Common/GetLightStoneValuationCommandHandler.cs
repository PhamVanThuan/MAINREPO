using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;

namespace DomainService2.SharedServices.Common
{
    public class GetLightStoneValuationCommandHandler : IHandlesDomainServiceCommand<GetLightStoneValuationCommand>
    {
        //WIP LB
        IPropertyRepository propertyRepository;

        public GetLightStoneValuationCommandHandler(IPropertyRepository propertyRepository)
        {
            this.propertyRepository = propertyRepository;
        }

        public void Handle(IDomainMessageCollection messages, GetLightStoneValuationCommand command)
        {
            IProperty property = propertyRepository.GetPropertyByApplicationKey(command.ApplicationKey);
            command.Result = false;
            if (property == null)
            {
                messages.Add(new DomainMessage("No property exists to do a valuation.", "No property exists to do a valuation."));
            }
            else
            {
                propertyRepository.GetLightStoneValuationForWorkFlow(command.ApplicationKey, command.ADUser);
                command.Result = true;
            }
        }
    }
}