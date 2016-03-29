using DomainService2.Workflow.Origination.Valuations;
using Machine.Fakes;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.Valuations.GetValuationData
{
    public class GetValuationDataCommandHandlerBase : WithFakes
    {
        protected static GetValuationDataCommand command;
        protected static GetValuationDataCommandHandler handler;
        protected static IDomainMessageCollection messages;
        protected static IApplicationReadOnlyRepository applicationRepository;

        protected static readonly int TEST_VALUATION_KEY = 1000;
        protected static readonly int TEST_PROPERTY_KEY = 2000;
        protected static readonly string TEST_PROPERTY_ID = "444";
        protected static readonly int VALUATION_DATA_PROVIDER_SERVICE_KEY = 90;

        protected static IProperty BuildPropertyWithLatestValuation(int valuationProviderKey)
        {
            IDataProvider dataProvider = An<IDataProvider>();
            dataProvider.WhenToldTo(x => x.Key).Return(valuationProviderKey);

            IDataProviderDataService dataProviderService = An<IDataProviderDataService>();
            dataProviderService.WhenToldTo(x => x.DataProvider).Return(dataProvider);

            IPropertyDataProviderDataService propertyDataProviderDataService = An<IPropertyDataProviderDataService>();
            propertyDataProviderDataService.WhenToldTo(x => x.DataProviderDataService).Return(dataProviderService);

            IPropertyData propertyData1 = An<IPropertyData>();
            propertyData1.WhenToldTo(x => x.PropertyID).Return(TEST_PROPERTY_ID);
            propertyData1.WhenToldTo(x => x.PropertyDataProviderDataService).Return(propertyDataProviderDataService);

            IEventList<IPropertyData> propertyDatas = new EventList<IPropertyData>();
            propertyDatas.Add(null, propertyData1);

            IProperty property = An<IProperty>();
            property.WhenToldTo(x => x.Key).Return(TEST_PROPERTY_KEY);
            property.WhenToldTo(x => x.PropertyDatas).Return(propertyDatas);

            IValuationDataProviderDataService valuationDataProviderService = An<IValuationDataProviderDataService>();
            valuationDataProviderService.WhenToldTo(x => x.Key).Return(VALUATION_DATA_PROVIDER_SERVICE_KEY);
            valuationDataProviderService.WhenToldTo(x => x.DataProviderDataService).Return(dataProviderService);

            IValuation valuation = An<IValuation>();
            valuation.WhenToldTo(x => x.Key).Return(TEST_VALUATION_KEY);
            valuation.WhenToldTo(x => x.Property).Return(property);
            valuation.WhenToldTo(x => x.ValuationDataProviderDataService).Return(valuationDataProviderService);

            property.WhenToldTo(x => x.LatestCompleteValuation).Return(valuation);

            return property;
        }
    }
}