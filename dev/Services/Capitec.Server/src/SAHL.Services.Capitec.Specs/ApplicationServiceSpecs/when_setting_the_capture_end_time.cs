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
    public class when_setting_the_capture_end_time : with_application_service
    {
        private static Guid applicationId;
        private static DateTime captureEndTime;

        Establish context = () =>
        {
            applicationId = Guid.NewGuid();
            captureEndTime = DateTime.Now;
        };

        Because of = () =>
        {
            applicationService.UpdateCaptureEndTime(applicationId, captureEndTime);
        };

        It should_set_the_capture_end_time = () =>
        {
            applicationDataService.WasToldTo(x => x.UpdateCaptureEndTime(applicationId, captureEndTime));
        };
    }
}
