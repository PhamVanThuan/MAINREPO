using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Workflow.Origination.Valuations
{
    public class SetValuationIsActiveCommandHandler : IHandlesDomainServiceCommand<SetValuationIsActiveCommand>
    {
        private IPropertyRepository propertyRepository;

        public SetValuationIsActiveCommandHandler(IPropertyRepository propertyRepository)
        {
            this.propertyRepository = propertyRepository;
        }

        public void Handle(IDomainMessageCollection messages, SetValuationIsActiveCommand command)
        {
            IValuation valuation = propertyRepository.GetValuationByKey(command.ValuationKey);
            foreach (IValuation v in valuation.Property.Valuations)
            {
                v.IsActive = false;
            }
            valuation.IsActive = true;
            propertyRepository.SaveValuation(valuation);
        }
    }
}