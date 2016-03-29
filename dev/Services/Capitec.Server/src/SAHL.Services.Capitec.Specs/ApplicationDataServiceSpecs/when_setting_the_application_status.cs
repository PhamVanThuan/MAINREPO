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
    public class when_setting_the_application_status : WithFakes
    {
        static ApplicationDataManager service;
        static Guid applicationId;
        static int applicationNumber;
        static Guid applicationStatusID;
        static DateTime statusChangeDate;
        private static FakeDbFactory dbFactory;

        Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            service = new ApplicationDataManager(dbFactory);
            applicationId = Guid.NewGuid();
            applicationStatusID = Guid.Parse(ApplicationStatusEnumDataModel.IN_PROGRESS);
            statusChangeDate = DateTime.Now;
        };

        Because of = () =>
        {
            service.SetApplicationStatus(applicationId, applicationStatusID, statusChangeDate);
        };

        It should_update_the_application_status_to_the_one_provided = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Update<ApplicationDataModel>(Arg.Is<SetApplicationStatusStatement>(y => 
                y.ApplicationStatusID == applicationStatusID && y.ApplicationID == applicationId && y.StatusChangeDate == statusChangeDate)));
        };
    }
}
