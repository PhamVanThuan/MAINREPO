using System;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.Capitec.Managers.Application;

namespace SAHL.Services.Capitec.Specs.ApplicationDataServiceSpecs
{
    public class when_getting_an_application_by_id : WithFakes
    {
        private static ApplicationDataManager service;
        private static Guid applicationID;
        private static ApplicationDataModel application;
        private static FakeDbFactory dbFactory;
        private static Guid branchId;

        private Establish context = () =>
        {
            applicationID = Guid.NewGuid();
            branchId = Guid.NewGuid();
            application = new ApplicationDataModel(applicationID, DateTime.Now, Guid.NewGuid(), 123, Guid.NewGuid(), null, null, Guid.NewGuid(), DateTime.Now, "", "", new DateTime(2014, 10, 10), null, branchId);
            dbFactory = new FakeDbFactory();
            service = new ApplicationDataManager(dbFactory);

            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.GetByKey<ApplicationDataModel, Guid>(applicationID)).Return(application);
        };

        private Because of = () =>
        {
            application = service.GetApplicationByID(applicationID);
        };

        private It should_return_the_application = () =>
        {
            application.ShouldNotBeNull();
            application.Id.ShouldEqual(applicationID);
        };
    }
}