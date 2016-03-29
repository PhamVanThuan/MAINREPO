using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Workflow.Origination.Valuations
{
    public class SetValuationStatusCommandHandler : IHandlesDomainServiceCommand<SetValuationStatusCommand>
    {
        IPropertyRepository propertyRepository;
        ILookupRepository lookupRepository;

        public SetValuationStatusCommandHandler(IPropertyRepository propertyRepository, ILookupRepository lookupRepository)
        {
            this.propertyRepository = propertyRepository;
            this.lookupRepository = lookupRepository;
        }

        public void Handle(IDomainMessageCollection messages, SetValuationStatusCommand command)
        {
            if (command.ValuationKey == 0)
            {
                return;
            }

            IValuation valuation = propertyRepository.GetValuationByKey(command.ValuationKey);

            valuation.ValuationStatus = lookupRepository.ValuationStatus.ObjectDictionary[(command.ValuationStatusKey).ToString()];
            propertyRepository.SaveValuation(valuation);
        }
    }
}