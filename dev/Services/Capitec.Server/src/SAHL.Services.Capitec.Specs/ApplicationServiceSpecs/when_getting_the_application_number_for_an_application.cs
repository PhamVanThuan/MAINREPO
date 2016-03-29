using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models.Capitec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Capitec.Specs.ApplicationServiceSpecs
{
    public class when_getting_the_application_number_for_an_application : with_application_service
    {
        private static ApplicationDataModel application;
        private static Guid applicationId;
        private static int? applicationNumber;
        private static Guid? guid;
        private static int applicationNumberResult;
        private static Guid branchId;

        Establish context = () =>
        {
            guid = null;
            branchId = Guid.NewGuid();
            applicationNumber = 1234567;
            applicationId = Guid.NewGuid();
            application = new ApplicationDataModel(applicationId, DateTime.Now, Guid.NewGuid(), applicationNumber, Guid.NewGuid(), guid, guid, Guid.NewGuid(),
                DateTime.Now, "Test", "0311234657", new DateTime(2014, 10, 10), null, branchId);
            applicationDataService.WhenToldTo(x => x.GetApplicationByID(applicationId)).Return(application);
        };

        Because of = () =>
        {
            applicationNumberResult = applicationService.GetApplicationNumberForApplication(applicationId);
        };

        It should_return_the_application_number_from_application_data_model = () =>
        {
            applicationNumber.ShouldEqual(applicationNumber);
        };
    }
}
