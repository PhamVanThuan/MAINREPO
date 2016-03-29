using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.DomainProcessManager.DomainProcesses.Managers.Application;
using SAHL.DomainProcessManager.DomainProcesses.Managers.Application.Statements;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.Managers.Application
{
   public class when_checking_for_non_existing_vendor : WithCoreFakes
    {
        private static ApplicationDataManager manager;
        private static FakeDbFactory dbFactory;
        private static string vendorcode = "AAA";
        private static bool result;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            manager = new ApplicationDataManager(dbFactory);
            dbFactory.FakedDb.InReadOnlyAppContext().WhenToldTo(x => x.SelectOne(Param<DoesSuppliedVendorExistStatement>.Matches(m =>
                m.VendorCode == vendorcode))).Return(0);
        };

        private Because of = () =>
        {
            result = manager.DoesSuppliedVendorExist(vendorcode);
        };

        private It should_select_whether_vendor_exists = () =>
        {
            dbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.SelectOne(Param<DoesSuppliedVendorExistStatement>.Matches(m =>
                 m.VendorCode == vendorcode)));
        };

        private It should_confirm_that_vendor_does_not_exists = () =>
        {
            result.ShouldBeFalse();
        };
    }
}