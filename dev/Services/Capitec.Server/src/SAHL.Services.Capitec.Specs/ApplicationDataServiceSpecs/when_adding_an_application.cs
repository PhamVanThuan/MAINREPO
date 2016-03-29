using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.Capitec.Managers.Application;
using System;

namespace SAHL.Services.Capitec.Specs.ApplicationDataServiceSpecs
{
    public class when_adding_an_application : WithFakes
    {
        private static ApplicationDataManager service;
        private static Guid applicationId, applicationPurposeEnumId, userId;
        private static DateTime applicationDate;
        private static int applicationNumber;
        private static Guid initialApplicationStatus;
        private static DateTime captureStartDate;
        private static FakeDbFactory dbFactory;
        private static Guid branchId;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            service = new ApplicationDataManager(dbFactory);
            applicationId = Guid.NewGuid();
            applicationPurposeEnumId = Guid.Parse(ApplicationPurposeEnumDataModel.SWITCH);
            initialApplicationStatus = Guid.Parse(ApplicationStatusEnumDataModel.IN_PROGRESS);
            userId = Guid.NewGuid();
            applicationDate = DateTime.Now;
            applicationNumber = 1234657;
            branchId = Guid.NewGuid();
            captureStartDate = new DateTime(2014, 10, 10);
        };

        private Because of = () =>
        {
            service.AddApplication(applicationId, applicationDate, applicationNumber, applicationPurposeEnumId, userId, captureStartDate, branchId);
        };

        private It should_insert_an_application_with_the_details_provided = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert(Arg.Is<ApplicationDataModel>(
                y => y.Id == applicationId
                && y.ApplicationDate == applicationDate
                && y.ApplicationNumber == applicationNumber
                && y.ApplicationPurposeEnumId == applicationPurposeEnumId
                && y.UserId == userId
                && y.CaptureStartTime == captureStartDate
                && y.CaptureEndTime == null
                && y.BranchId == branchId
                )));
        };

        private It should_set_the_initial_application_status_to_IN_PROGRESS = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert(Arg.Is<ApplicationDataModel>(y => y.ApplicationStatusEnumId == initialApplicationStatus)));
        };
    }
}