using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.ApplicationDomain.Managers.Domicilium;
using SAHL.Services.ApplicationDomain.Managers.Domicilium.Statements;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.Domicilium
{
    public class when_domicilium_address_is_not_pending_domicilium_address : WithFakes
    {
        static IDomiciliumDataManager domiciliumDataManager;
        private static FakeDbFactory dbFactory;
        static int clientDomiciliumKey, expectedResult;
        static bool response;

        Establish context = () =>
        {
            clientDomiciliumKey = 50;
            expectedResult = 0;
            dbFactory = new FakeDbFactory();
            domiciliumDataManager = new DomiciliumDataManager(dbFactory);

            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.SelectOne<int>(Param.IsAny<IsDomiciliumAddressPendingDomiciliumAddressStatement>())).Return(() => { return expectedResult; });
        };

        Because of = () =>
        {
            response = domiciliumDataManager.IsDomiciliumAddressPendingDomiciliumAddress(clientDomiciliumKey);
        };

        It should_check_if_domicilium_address_is_pending_domicilium_address = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.SelectOne<int>(Param.IsAny<IsDomiciliumAddressPendingDomiciliumAddressStatement>()));
        };

        It should_confirm_that_domcilium_address_is_not_pending_domicilium_address = () =>
        {
            response.ShouldBeFalse();
        };
    }
}
