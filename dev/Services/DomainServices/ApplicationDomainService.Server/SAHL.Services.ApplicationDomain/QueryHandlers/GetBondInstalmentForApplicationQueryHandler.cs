using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.Interfaces.ApplicationDomain.Queries;
using System;
using System.Collections.Generic;

namespace SAHL.Services.ApplicationDomain.QueryHandlers
{
    public class GetBondInstalmentForApplicationQueryHandler : IServiceQueryHandler<GetBondInstalmentForApplicationQuery>
    {
        private IAffordabilityAssessmentManager affordabilityAssessmentManager;
        private IApplicationDataManager applicationDataManager;
        private IDictionary<OfferType, Func<int, double?>> InstalmentCalculatorMap;

        public GetBondInstalmentForApplicationQueryHandler(IApplicationDataManager applicationDataManager, IAffordabilityAssessmentManager affordabilityAssessmentManager)
        {
            this.affordabilityAssessmentManager = affordabilityAssessmentManager;
            this.applicationDataManager = applicationDataManager;

            InstalmentCalculatorMap = new Dictionary<OfferType, Func<int, double?>>
            {
                { OfferType.NewPurchaseLoan, affordabilityAssessmentManager.GetBondInstalmentForNewBusinessApplication },
                { OfferType.SwitchLoan, affordabilityAssessmentManager.GetBondInstalmentForNewBusinessApplication },
                { OfferType.RefinanceLoan, affordabilityAssessmentManager.GetBondInstalmentForNewBusinessApplication },
                { OfferType.FurtherAdvance, affordabilityAssessmentManager.GetBondInstalmentForFurtherLendingApplication },
                { OfferType.FurtherLoan, affordabilityAssessmentManager.GetBondInstalmentForFurtherLendingApplication },
                { OfferType.Re_Advance, affordabilityAssessmentManager.GetBondInstalmentForFurtherLendingApplication },
            };
        }

        public ISystemMessageCollection HandleQuery(GetBondInstalmentForApplicationQuery query)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();

            OfferDataModel offerDataModel = applicationDataManager.GetApplication(query.ApplicationKey);
            double? result = InstalmentCalculatorMap[(OfferType)offerDataModel.OfferTypeKey](query.ApplicationKey);
            query.Result = new ServiceQueryResult<double?>(new List<double?> { result });

            return messages;
        }
    }
}