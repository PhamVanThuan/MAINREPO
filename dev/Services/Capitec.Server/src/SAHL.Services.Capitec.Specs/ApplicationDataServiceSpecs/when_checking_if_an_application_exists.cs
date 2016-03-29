using System;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;

using SAHL.Services.Capitec.Managers.Application.Statements;
using SAHL.Services.Capitec.Managers.Application;
using SAHL.Core.Testing.Providers;
using SAHL.Core.Testing.Fakes;

namespace SAHL.Services.Capitec.Specs.ApplicationDataServiceSpecs
{
    public class when_checking_if_an_application_exists : WithFakes
    {
        private static ApplicationDataManager service;
        private static Guid applicationID;
        private static bool result;
        private static FakeDbFactory dbFactory;

        Establish context = () =>
        {
            applicationID = Guid.NewGuid();
            dbFactory = new FakeDbFactory();
            service = new ApplicationDataManager(dbFactory);

            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.SelectOne(Param.IsAny<GetApplicationExistsQuery>())).Return(1);
        };

        Because of = () =>
        {
            result = service.DoesApplicationExist(applicationID);
        };

        It should_return_that_the_application_exists = () =>
        {
            result.ShouldBeTrue();
        };
    }
}
