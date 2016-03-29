using System;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Workflow.Origination.Valuations
{
    public class GetValuationDataCommandHandler : IHandlesDomainServiceCommand<GetValuationDataCommand>
    {
        private IApplicationReadOnlyRepository applicationReadOnlyRepository;

        public GetValuationDataCommandHandler(IApplicationReadOnlyRepository applicationReadOnlyRepository)
        {
            if (applicationReadOnlyRepository == null)
            {
                throw new ArgumentNullException(Strings.ArgApplicationReadOnlyRepository);
            }

            this.applicationReadOnlyRepository = applicationReadOnlyRepository;
        }

        public void Handle(IDomainMessageCollection messages, GetValuationDataCommand command)
        {
            IApplication app = applicationReadOnlyRepository.GetApplicationByKey(command.ApplicationKey);

            IApplicationMortgageLoan applicationMortgageLoan = app as IApplicationMortgageLoan;

            command.ValuationDataResult = new Dictionary<string, object>();

            if (applicationMortgageLoan == null)
            {
                return;
            }

            if (applicationMortgageLoan.Property == null)
            {
                return;
            }

            IValuation latestValuation = applicationMortgageLoan.Property.LatestCompleteValuation;

            if (latestValuation != null)
            {
                command.ValuationDataResult.Add("ValuationKey", latestValuation.Key);
                command.ValuationDataResult.Add("PropertyKey", latestValuation.Property.Key);
                command.ValuationDataResult.Add("ValuationDataProviderDataServiceKey", latestValuation.ValuationDataProviderDataService.Key);

                if (latestValuation.Property.PropertyDatas != null)
                {
                    foreach (IPropertyData pd in latestValuation.Property.PropertyDatas)
                    {
                        if (pd.PropertyDataProviderDataService.DataProviderDataService.Key == latestValuation.ValuationDataProviderDataService.DataProviderDataService.Key)
                        {
                            switch (pd.PropertyDataProviderDataService.DataProviderDataService.DataProvider.Key)
                            {
                                case (int)SAHL.Common.Globals.DataProviders.AdCheck:
                                    command.ValuationDataResult.Add("AdcheckPropertyID", pd.PropertyID);
                                    break;

                                case (int)SAHL.Common.Globals.DataProviders.LightStone:
                                    command.ValuationDataResult.Add("LightstonePropertyID", pd.PropertyID);
                                    break;

                                default:
                                    break;
                            }
                            break;
                        }
                    }
                }
            }
        }
    }
}