using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.ApplicationDomain.Managers.Application;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.ApplicationData
{
    public class when_updating_an_applicantion_information_variable_loan : WithFakes
    {
        private static IApplicationDataManager applicationDataManager;
        private static FakeDbFactory dbFactory;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            applicationDataManager = new ApplicationDataManager(dbFactory);
        };

        private Because of = () =>
        {
            applicationDataManager.UpdateApplicationInformationVariableLoan(Param.IsAny<OfferInformationVariableLoanDataModel>());
        };

        private It should_insert_the_applicant = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Update<OfferInformationVariableLoanDataModel>(Param.IsAny<OfferInformationVariableLoanDataModel>()));
        };
    }
}