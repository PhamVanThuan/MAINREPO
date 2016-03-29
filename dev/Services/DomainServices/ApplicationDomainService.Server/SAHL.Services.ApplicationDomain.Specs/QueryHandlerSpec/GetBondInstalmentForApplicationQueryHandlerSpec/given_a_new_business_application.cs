using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.ApplicationDomain.QueryHandlers;
using SAHL.Services.Interfaces.ApplicationDomain.Queries;

namespace SAHL.Services.ApplicationDomain.Specs.QueryHandlerSpec.GetBondInstalmentForApplicationQueryHandlerSpec
{
    public class given_a_new_business_application : WithFakes
    {
        private static GetBondInstalmentForApplicationQuery query;
        private static GetBondInstalmentForApplicationQueryHandler handler;
        private static IAffordabilityAssessmentManager affordabilityAssessmentManager;
        private static IApplicationDataManager applicationDataManager;
        private static OfferDataModel offerDataModel;

        private Establish context = () =>
        {
            applicationDataManager = An<IApplicationDataManager>();
            offerDataModel = new OfferDataModel((int)OfferType.NewPurchaseLoan, (int)OfferStatus.Open, null, null, null, null, null, null, 0, 0, null);
            applicationDataManager.WhenToldTo(x => x.GetApplication(Param.IsAny<int>())).Return(offerDataModel);

            affordabilityAssessmentManager = An<IAffordabilityAssessmentManager>();
            affordabilityAssessmentManager.WhenToldTo(x => x.GetBondInstalmentForNewBusinessApplication(Param.IsAny<int>())).Return(Param.IsAny<double>());

            query = new GetBondInstalmentForApplicationQuery(Param.IsAny<int>());
            handler = new GetBondInstalmentForApplicationQueryHandler(applicationDataManager, affordabilityAssessmentManager);
        };

        private Because of = () =>
        {
            handler.HandleQuery(query);
        };

        private It should_get_the_application = () =>
        {
            applicationDataManager.WasToldTo(x => x.GetApplication(Param.IsAny<int>()));
        };

        private It should_get_the_bond_installment_for_new_business = () =>
        {
            affordabilityAssessmentManager.WasToldTo(x => x.GetBondInstalmentForNewBusinessApplication(0));
        };
    }
}