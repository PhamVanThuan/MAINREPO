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
    public class when_saving_an_applicant_role : WithFakes
    {
        private static IApplicantDataManager applicantDataManager;
        private static FakeDbFactory dbFactory;

        private static OfferRoleDataModel applicantRole;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            applicantDataManager = new ApplicantDataManager(dbFactory);

            applicantRole = new OfferRoleDataModel(12345, 54321, 8, 1, DateTime.Now);

        };

        private Because of = () =>
        {
            applicantDataManager.AddApplicantRole(applicantRole);
        };

        private It should_insert_the_applicant_role = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert<OfferRoleDataModel>(applicantRole));
        };
    }
}