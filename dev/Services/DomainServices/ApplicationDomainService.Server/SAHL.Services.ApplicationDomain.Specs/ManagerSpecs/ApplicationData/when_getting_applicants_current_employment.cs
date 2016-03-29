using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.ApplicationDomain.Managers.Application.Statements;
using System.Collections.Generic;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.ApplicationData
{
    public class when_getting_applicants_current_employment : WithFakes
    {
        private static IApplicantDataManager applicantDataManager;
        private static IEnumerable<EmploymentDataModel> employment;
        private static FakeDbFactory dbFactory;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            applicantDataManager = new ApplicantDataManager(dbFactory);
        };

        private Because of = () =>
        {
            employment = applicantDataManager.GetIncomeContributorApplicantsCurrentEmployment(Param.IsAny<int>());
        };

        private It should_select_a_legalentity_data_model = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.Select<EmploymentDataModel>(Param.IsAny<GetIncomeContributorApplicantsCurrentEmploymentStatement>()));
        };
    }
}