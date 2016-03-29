using System;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models.Capitec;

namespace SAHL.Services.Capitec.Specs.ApplicationServiceSpecs
{
    public class when_getting_the_application_number_and_it_does_not_exist : with_application_service
    {
        private static ApplicationDataModel application;
        private static Guid applicationId;
        private static int? applicationNumber;
        private static Guid? guid;
        private static int applicationNumberResult;
        private static Guid branchId;

        private Establish context = () =>
        {
            guid = null;
            branchId = Guid.NewGuid();
            applicationNumber = null;
            applicationId = Guid.NewGuid();
            application = new ApplicationDataModel(applicationId, DateTime.Now, Guid.NewGuid(), applicationNumber, Guid.NewGuid(), guid, guid, Guid.NewGuid(),
                DateTime.Now, "Test", "0311234657", new DateTime(2014, 10, 10), null, branchId);
            applicationDataService.WhenToldTo(x => x.GetApplicationByID(applicationId)).Return(application);
        };

        private Because of = () =>
        {
            applicationNumberResult = applicationService.GetApplicationNumberForApplication(applicationId);
        };

        private It should_return_minus_one = () =>
        {
            applicationNumberResult.ShouldEqual(-1);
        };
    }
}