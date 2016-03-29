using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using System;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.ApplicationData
{
    public class when_saving_an_applicant : WithFakes
    {
        private static IApplicantDataManager applicantDataManager;
        private static FakeDbFactory dbFactory;

        private static int legalEntityKey, expectedLegalEntityKey;
        private static LegalEntityDataModel applicant;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            applicantDataManager = new ApplicantDataManager(dbFactory);
            expectedLegalEntityKey = 12345;

            applicant = new LegalEntityDataModel(2, 6, 2, 2, DateTime.Now, 2, "bob", "bs", "smith", null, "8001045000007", null, null, null, null, null, DateTime.Now.AddYears(-35),
                null, null, null, null, null, null, null, null, null, 1, 1, null, null, null, null, null, 2, 2, null);

            dbFactory.FakedDb.DbContext.WhenToldTo<IReadWriteSqlRepository>(x => x.Insert<LegalEntityDataModel>(Param.IsAny<LegalEntityDataModel>()))
                .Callback<LegalEntityDataModel>(y => { y.LegalEntityKey = expectedLegalEntityKey; });
        };

        private Because of = () =>
        {
            legalEntityKey = applicantDataManager.SaveApplicant(applicant);
        };

        private It should_insert_the_applicant = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert<LegalEntityDataModel>(applicant));
        };

        private It should_return_a_list_of_applicants = () =>
        {
            legalEntityKey.ShouldEqual(expectedLegalEntityKey);
        };
    }
}