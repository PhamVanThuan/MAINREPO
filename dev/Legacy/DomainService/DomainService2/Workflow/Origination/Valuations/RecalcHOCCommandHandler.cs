using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Workflow.Origination.Valuations
{
    public class RecalcHOCCommandHandler : IHandlesDomainServiceCommand<RecalcHOCCommand>
    {
        private IPropertyRepository propertyRepository;

        public RecalcHOCCommandHandler(IPropertyRepository propertyRepository)
        {
            this.propertyRepository = propertyRepository;
        }

        public void Handle(IDomainMessageCollection messages, RecalcHOCCommand command)
        {
            IValuation valuation = propertyRepository.GetValuationByKey(command.ValuationKey);

            if (valuation.ValuationDataProviderDataService.DataProviderDataService.Key == (int)SAHL.Common.Globals.DataProviderDataServices.AdCheckPhysicalValuation)
                propertyRepository.AdcheckValuationUpdateHOC(command.ValuationKey, command.ApplicationKey);
            else
                propertyRepository.ValuationUpdateHOC(command.ValuationKey, SAHL.Common.Globals.GenericKeyTypes.Offer, command.ApplicationKey);
        }
    }
}