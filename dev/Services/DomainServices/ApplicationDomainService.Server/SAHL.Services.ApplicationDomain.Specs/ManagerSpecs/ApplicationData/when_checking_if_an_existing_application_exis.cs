﻿using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.ApplicationDomain.Managers.Application.Statements;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.ApplicationData
{
    public class when_checking_if_an_existing_application_exist : WithFakes
    {
        private static IApplicationDataManager applicationDataManager;
        private static FakeDbFactory dbFactory;
        private static bool result;
        private static int applicationNumber;

        private Establish context = () =>
        {
            applicationNumber = 1408282;
            dbFactory = new FakeDbFactory();
            applicationDataManager = new ApplicationDataManager(dbFactory);
            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.Select<int>(Param.IsAny<DoesOpenApplicationExistStatement>())).Return(new int[] { 1 });
        };

        private Because of = () =>
        {
            result = applicationDataManager.DoesOpenApplicationExist(applicationNumber);
        };

        private It should_query_for_application_mailing_address_using_applicationNumber = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.Select<int>(Arg.Is<DoesOpenApplicationExistStatement>(y => y.ApplicationNumber == applicationNumber)));
        };

        private It should_return_true = () =>
       {
           result.ShouldBeTrue();
       };
    }
}