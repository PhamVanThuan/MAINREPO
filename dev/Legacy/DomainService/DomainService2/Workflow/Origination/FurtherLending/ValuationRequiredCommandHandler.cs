using System;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace DomainService2.Workflow.Origination.FurtherLending
{
    public class ValuationRequiredCommandHandler : IHandlesDomainServiceCommand<ValuationRequiredCommand>
    {
        private IApplicationRepository applicationRepository;

        public ValuationRequiredCommandHandler(IApplicationRepository applicationRepository)
        {
            this.applicationRepository = applicationRepository;
        }

        public void Handle(IDomainMessageCollection messages, ValuationRequiredCommand command)
        {
            command.Result = false;
            IApplicationMortgageLoan appMortgageLoan = applicationRepository.GetApplicationByKey(command.ApplicationKey)
                                                        as IApplicationMortgageLoan;
            if (appMortgageLoan.ApplicationType.Key == (int)OfferTypes.FurtherAdvance ||
                appMortgageLoan.ApplicationType.Key == (int)OfferTypes.FurtherLoan)
            {
                IValuation latestValuation = appMortgageLoan.Property.LatestCompleteValuation;
                // if FL valuation is valid for 24 months
                int monthsValid = 24;
                // if further advance valuation is valid for 36 months
                if (appMortgageLoan.ApplicationType.Key == (int)OfferTypes.FurtherAdvance)
                    monthsValid = 36;

                if (((DateTime)latestValuation.ValuationDate).AddMonths(monthsValid) < DateTime.Now)
                {
                    command.Result = true;
                }
            }
        }
    }
}