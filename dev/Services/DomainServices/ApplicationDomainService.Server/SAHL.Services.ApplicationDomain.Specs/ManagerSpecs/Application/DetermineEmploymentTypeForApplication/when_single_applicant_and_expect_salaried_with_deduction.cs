using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.ApplicationDomain.Managers.Application;
using System.Collections.Generic;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.Application
{
    public class when_single_applicant_and_expect_salaried_with_deduction : WithFakes
    {
        private static IApplicationManager applicationManager;
        private static IEnumerable<EmploymentDataModel> employment;
        private static EmploymentType actualEmploymentType;
        private static EmploymentType expectedEmploymentType;
        private static IApplicationDataManager applicationDataManager;

        private Establish context = () =>
        {
            applicationDataManager = An<IApplicationDataManager>();

            expectedEmploymentType = EmploymentType.SalariedwithDeduction;
            employment = new List<EmploymentDataModel>()
            {
                new EmploymentDataModel(0, null, (int)EmploymentType.SalariedwithDeduction, (int)RemunerationType.Salaried, (int)EmploymentStatus.Current, 1, null, null, "", "", "", "", null, 
                    "", null, "", 7, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "", 0, 0, null, null, null, null, null)
            };
            applicationManager = new ApplicationManager(applicationDataManager);
        };

        private Because of = () =>
        {
            actualEmploymentType = applicationManager.DetermineEmploymentTypeForApplication(employment);
        };

        private It should_return_salaried_with_deduction = () =>
        {
            actualEmploymentType.ShouldEqual(expectedEmploymentType);
        };
    }
}