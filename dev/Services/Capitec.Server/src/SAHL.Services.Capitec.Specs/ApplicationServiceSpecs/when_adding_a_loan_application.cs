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
    public class when_adding_a_loan_application : with_application_service
    {
        private static LoanApplication application;
        private static DateTime applicationDate;
        private static Guid applicationPurposeEnumId;
        private static Guid applicationID;
        private static int applicationNumber;
        private static DateTime captureStartTime;
        private static Guid branchId;

        private Establish context = () =>
        {
            applicationNumber = 1;
            applicationPurposeEnumId = Guid.Parse(ApplicationPurposeEnumDataModel.SWITCH);
            applicationID = Guid.NewGuid();
            captureStartTime = new DateTime(2014, 10, 10);
            branchId = Guid.NewGuid();

            var switchApplication = new SwitchLoan(150000, 500000, 30000000, 150000, Guid.Parse(OccupancyTypeEnumDataModel.OWNER_OCCUPIED), Guid.Parse(EmploymentTypeEnumDataModel.SALARIED), new List<Applicant>()).GetSwitchLoanApplication;
            application = new LoanApplication(switchApplication.ApplicationDate, new ApplicationLoanDetails(switchApplication.SwitchLoanDetails), switchApplication.Applicants, switchApplication.UserId, captureStartTime, branchId);
            applicationDataService.WhenToldTo(x => x.GetNextApplicationNumber()).Return(applicationNumber);
        };

        private Because of = () =>
        {
            applicationService.AddLoanApplication(applicationNumber, applicationID, application);
        };

        private It should_store_the_application = () =>
        {
            applicationDataService.WasToldTo(x => x.AddApplication(applicationID, application.ApplicationDate, applicationNumber, applicationPurposeEnumId, application.UserId, captureStartTime, branchId));
        };
    }
}