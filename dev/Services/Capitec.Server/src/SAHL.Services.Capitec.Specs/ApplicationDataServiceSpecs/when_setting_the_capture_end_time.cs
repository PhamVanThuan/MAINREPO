using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;

using System;
using SAHL.Services.Capitec.Managers.Application.Statements;
using SAHL.Services.Capitec.Managers.Application;
using SAHL.Core.Testing.Providers;
using SAHL.Core.Testing.Fakes;

namespace SAHL.Services.Capitec.Specs.ApplicationDataServiceSpecs
{
    public class when_setting_the_capture_end_time : WithFakes
    {
        static ApplicationDataManager service;
        static Guid applicationId;
        static int applicationNumber;
        static DateTime captureEndTime;
        private static FakeDbFactory dbFactory;

        Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            service = new ApplicationDataManager(dbFactory);
            applicationId = Guid.NewGuid();
            captureEndTime = DateTime.Now;
        };

        Because of = () =>
        {
            service.UpdateCaptureEndTime(applicationId, captureEndTime);
        };

        It should_update_the_application_capture_end_time_to_the_one_provided = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Update<ApplicationDataModel>(Arg.Is<SetApplicationCaptureEndTime>(y => 
                y.CaptureEndTime == captureEndTime && y.ApplicationID == applicationId)));
        };
    }
}
