using System;
using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Services.Capitec.Managers.Application.Models;
using SAHL.Services.Capitec.Specs.Stubs;
using SAHL.Services.Interfaces.Capitec.ViewModels.Apply;

namespace SAHL.Services.Capitec.Specs.ApplicationServiceSpecs
{
    public class when_adding_a_loan_application_and_application_exists : with_application_service
    {
        private static LoanApplication application;
        private static DateTime applicationDate;
        private static Guid applicationPurposeEnumId;
        private static Guid applicationID;
        private static int applicationNumber;
        private static Guid branchId;

        private Establish context = () =>
        {
            applicationNumber = 1;
            applicationPurposeEnumId = Guid.Parse(ApplicationPurposeEnumDataModel.SWITCH);
            applicationID = Guid.NewGuid();
            branchId = Guid.NewGuid();
            var switchApplication = new SwitchLoan(150000, 500000, 30000000, 150000, Guid.Parse(OccupancyTypeEnumDataModel.OWNER_OCCUPIED), Guid.Parse(EmploymentTypeEnumDataModel.SALARIED), new List<Applicant>()).GetSwitchLoanApplication;
            application = new LoanApplication(switchApplication.ApplicationDate, new ApplicationLoanDetails(switchApplication.SwitchLoanDetails), switchApplication.Applicants, switchApplication.UserId, new DateTime(2014, 10, 10), branchId);
            applicationDataService.WhenToldTo(x => x.DoesApplicationExist(applicationID)).Return(true);
            applicationDataService.WhenToldTo(x => x.GetNextApplicationNumber()).Return(applicationNumber);
        };

        private Because of = () =>
        {
            applicationService.AddLoanApplication(applicationNumber, applicationID, application);
        };

        private It should_not_save_the_application = () =>
        {
            applicationDataService.WasNotToldTo(x => x.AddApplication(Param.IsAny<Guid>(), Param.IsAny<DateTime>(), Param.IsAny<int>(), Param.IsAny<Guid>(), Param.IsAny<Guid>(), Param.IsAny<DateTime>(), Param.IsAny<Guid>()));
        };
    }
}