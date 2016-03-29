﻿using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.WCFServices.ComcorpConnector.Managers.ComcorpApplication;
using SAHL.WCFServices.ComcorpConnector.Managers.ComcorpApplication.Statements;

namespace SAHL.WCFServices.ComcorpConnector.Specs.ManagerSpecs.ComcorpApplicationData
{
    public class when_getting_app_number_for_comcorp_code_and_exists : WithCoreFakes
    {
        private static ComcorpApplicationDataManager dataManager;
        private static FakeDbFactory fakeDbFactory;
        private static long applicationCode;
        private static int? applicationNumber;
        private static int expectedApplicationNumber;

        private Establish context = () =>
        {
            fakeDbFactory = new FakeDbFactory();
            dataManager = new ComcorpApplicationDataManager(fakeDbFactory);
            applicationCode = 1121312;
            expectedApplicationNumber = 3324;
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WhenToldTo(x => x.SelectOne(
                Param.IsAny<GetAppNumberForComcorpAppCodeStatement>())).Return(expectedApplicationNumber);
        };

        private Because of = () =>
        {
            applicationNumber = dataManager.GetApplicationNumberForApplicationCode(applicationCode);
        };

        private It should_query_for_the_application_number = () =>
        {
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.SelectOne(
                Param<GetAppNumberForComcorpAppCodeStatement>.Matches(m => m.ComcorpApplicationCode == applicationCode.ToString())));
        };

        private It should_return_the_application_number = () =>
        {
            applicationNumber.Value.ShouldEqual(expectedApplicationNumber);
        };
    }
}