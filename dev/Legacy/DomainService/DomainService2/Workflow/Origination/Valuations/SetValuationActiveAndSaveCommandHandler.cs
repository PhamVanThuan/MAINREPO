using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Workflow.Origination.Valuations
{
    public class SetValuationActiveAndSaveCommandHandler : IHandlesDomainServiceCommand<SetValuationActiveAndSaveCommand>
    {
        private IPropertyRepository propertyRepository;

        public SetValuationActiveAndSaveCommandHandler(IPropertyRepository propertyRepository)
        {
            this.propertyRepository = propertyRepository;
        }

        public void Handle(IDomainMessageCollection messages, SetValuationActiveAndSaveCommand command)
        {
            IValuation val = propertyRepository.GetValuationByKey(command.ValuationKey);
            val.IsActive = true;
            propertyRepository.SaveValuation(val);
        }
    }
}