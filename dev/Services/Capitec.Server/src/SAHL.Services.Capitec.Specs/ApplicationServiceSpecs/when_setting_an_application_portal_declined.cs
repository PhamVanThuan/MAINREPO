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
    public class when_setting_an_application_portal_declined : with_application_service
    {
        private static Guid applicationId;
        private static Guid portalDeclineStatusEnumId;

        Establish context = () =>
        {
            applicationId = Guid.NewGuid();
            portalDeclineStatusEnumId = Guid.Parse(ApplicationStatusEnumDataModel.PORTAL_DECLINED);
        };

        Because of = () =>
        {
            applicationService.SetApplicationToDeclined(applicationId);
        };

        It should_use_the_data_service_to_set_application_to_declined = () =>
        {
            applicationDataService.WasToldTo(x => x.SetApplicationStatus(applicationId, portalDeclineStatusEnumId, Param.IsAny<DateTime>()));
        };
    }
}
