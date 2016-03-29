using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.ApplicationDomain.Managers.Application;
using System.Collections.Generic;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.Application
{
    public class when_single_applicant_and_expected_salaried : WithFakes
    {
        private static IApplicationManager applicationManager;
        private static IEnumerable<EmploymentDataModel> employment;
        private static EmploymentType actualEmploymentType;
        private static EmploymentType expectedEmploymentType;
        private static IApplicationDataManager applicationDataManager;

        private Establish context = () =>
        {
            applicationDataManager = An<IApplicationDataManager>();

            expectedEmploymentType = EmploymentType.Salaried;
            employment = new List<EmploymentDataModel>()
            {
                new EmploymentDataModel(0, null, (int)EmploymentType.Salaried, (int)RemunerationType.Salaried, (int)EmploymentStatus.Current, 1, null, null, "", "", "", "", null, "", null,
                    "", 3, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "", 0, 0, null, null, null, null, null)
            };
            applicationManager = new ApplicationManager(applicationDataManager);
        };

        private Because of = () =>
        {
            actualEmploymentType = applicationManager.DetermineEmploymentTypeForApplication(employment);
        };

        private It should_return_salaried = () =>
        {
            actualEmploymentType.ShouldEqual(expectedEmploymentType);
        };
    }
}