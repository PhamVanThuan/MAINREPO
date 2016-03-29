using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.ApplicationDomain.Managers.Applicant;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.ApplicationData
{
    public class when_updating_an_applicant : WithFakes
    {
        private static IApplicantDataManager applicantDataManager;
        private static FakeDbFactory dbFactory;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            applicantDataManager = new ApplicantDataManager(dbFactory);
        };

        private Because of = () =>
        {
            applicantDataManager.UpdateApplicant(Param.IsAny<LegalEntityDataModel>());
        };

        private It should_insert_the_applicant = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Update<LegalEntityDataModel>(Param.IsAny<LegalEntityDataModel>()));
        };
    }
}