using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.Capitec.Managers.Employment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Capitec.Specs.EmploymentDataServiceSpecs
{
    public class when_adding_applicant_employment : WithFakes
    {
        private static FakeDbFactory dbFactory;
        private static EmploymentDataManager service;
        private static Guid applicantId, employmentTypeEnumId;
        private static decimal basicMonthlyIncome, _3monthAverageCommission, housingAllowance;

        Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            service = new EmploymentDataManager(dbFactory);
            applicantId = Guid.NewGuid();
            employmentTypeEnumId = Guid.NewGuid();
            basicMonthlyIncome = 2000M;
            _3monthAverageCommission = 1000M;
            housingAllowance = 3000M;
        };

        Because of = () =>
        {
            service.AddApplicantEmployment(applicantId, employmentTypeEnumId, basicMonthlyIncome, _3monthAverageCommission, housingAllowance);
        };

        It should_add_an_applicant_employment_record_with_the_details_provided = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert(Arg.Is<ApplicantEmploymentDataModel>(
                f => f.ApplicantId == applicantId
                    && f.BasicIncome == basicMonthlyIncome
                    && f.ThreeMonthAverageCommission == _3monthAverageCommission
                    && f.HousingAllowance == housingAllowance
                )));
        };
    }
}
