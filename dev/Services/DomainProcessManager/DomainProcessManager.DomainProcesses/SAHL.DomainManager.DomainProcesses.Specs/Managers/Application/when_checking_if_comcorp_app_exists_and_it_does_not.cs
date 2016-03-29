using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.DomainProcessManager.DomainProcesses.Managers.Application;
using SAHL.DomainProcessManager.DomainProcesses.Managers.Application.Statements;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;


namespace SAHL.DomainProcessManager.DomainProcesses.Specs.Managers.Application
{
    internal class when_checking_if_comcorp_app_exists_and_it_does_not : WithCoreFakes
    {
        private static ApplicationDataManager manager;
        private static FakeDbFactory dbFactory;
        private static string clientIdNumber = "1234567891234";
        private static bool result;
        private static ComcorpApplicationPropertyDetailsModel property;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            manager = new ApplicationDataManager(dbFactory);
            property = ApplicationCreationTestHelper.PopulateComcorpPropertyDetailsModel();
            dbFactory.FakedDb.InReadOnlyAppContext().WhenToldTo(x => x.SelectOne(Param<DoesOpenAppExistForComcorpPropertyAndClientStatement>.Matches(m =>
                m.ClientIdNumber == clientIdNumber))).Return(0);
        };

        private Because of = () =>
        {
            result = manager.DoesOpenApplicationExistForComcorpProperty(clientIdNumber, property);
        };

        private It should_select_whether_the_offer_exists = () =>
        {
            dbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.SelectOne(Param<DoesOpenAppExistForComcorpPropertyAndClientStatement>.Matches(m =>
                 m.City == property.City &&
                 m.StreetNo == property.StreetNo &&
                 m.StreetName == property.StreetName &&
                 m.ClientIdNumber == clientIdNumber)));
        };

        private It should_return_that_the_offer_does_not_exist = () =>
        {
            result.ShouldBeFalse();
        };
    }
}