using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace DomainService2.Workflow.Origination.Valuations
{
    public class SetValuationStatusReturnedCommandHandler : IHandlesDomainServiceCommand<SetValuationStatusReturnedCommand>
    {
        private IPropertyRepository propertyRepository;
        private ILookupRepository lookupRepository;

        public SetValuationStatusReturnedCommandHandler(IPropertyRepository propertyRepository, ILookupRepository lookupRepository)
        {
            this.propertyRepository = propertyRepository;
            this.lookupRepository = lookupRepository;
        }

        public void Handle(IDomainMessageCollection messages, SetValuationStatusReturnedCommand command)
        {
            IValuation valuation = propertyRepository.GetValuationByKey(command.ValuationKey);

            if (valuation != null)
            {
                if (valuation.ValuationStatus.Key == (int)ValuationStatuses.Pending)
                {
                    valuation.ValuationStatus = lookupRepository.ValuationStatus.ObjectDictionary[((int)ValuationStatuses.Returned).ToString()];
                    propertyRepository.SaveValuation(valuation);
                }
            }
        }
    }
}