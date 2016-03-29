using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.ApplicationDomain.Managers.Application.Statements;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.Application
{
    public class when_linking_property_to_application : WithFakes
    {
        static IApplicationDataManager applicationDataManager;
        static int applicationNumber, propertyKey;
        static FakeDbFactory dbFactory;

        Establish context = () =>
        {
            applicationNumber = 1256;
            propertyKey = 24;

            dbFactory = new FakeDbFactory();
            applicationDataManager = new ApplicationDataManager(dbFactory);
        };

        Because of = () =>
        {
            applicationDataManager.LinkPropertyToApplication(applicationNumber, propertyKey);
        };

        It should_link_the_property_to_the_application = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Update<OfferMortgageLoanDataModel>(Param.IsAny<LinkPropertyToApplicationStatement>()));
        };
    }
}
