using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.ApplicationDomain.Managers.Application;
using System;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.Application
{
    public class when_getting_an_applicationinformationvariableloan : WithFakes
    {
        private static IApplicationDataManager applicationDataManager;
        private static FakeDbFactory dbFactory;
        private static int applicationInformationNumber;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            applicationDataManager = new ApplicationDataManager(dbFactory);
            applicationInformationNumber = 123;
        };

        private Because of = () =>
        {
            applicationDataManager.GetApplicationInformationVariableLoan(applicationInformationNumber);
        };

        private It should_get_the_applicationinformationvariableloan = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.GetByKey<OfferInformationVariableLoanDataModel, int>(applicationInformationNumber));
        };
    }
}