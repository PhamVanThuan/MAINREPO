using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.DomainServiceChecks.Managers.AccountDataManager;
using SAHL.DomainServiceChecks.Managers.AccountDataManager.Statements;
using SAHL.DomainServiceChecks.Managers.X2InstanceDataManager;
using SAHL.DomainServiceChecks.Managers.X2InstanceDataManager.Statement;

namespace SAHL.DomainServiceCheck.Specs.Managers.X2DataManagerSpecs
{
    public class when_checking_for_a_non_instance : WithCoreFakes
    {
        private static IX2DataManager X2DataManager;
        private static int InstanceId;
        private static bool InstanceExistsResponse;
        private static FakeDbFactory fakeDbFactory;

        private Establish context = () =>
        {
            fakeDbFactory = new FakeDbFactory();
            InstanceId = 1;
            X2DataManager = new X2DataManager(fakeDbFactory);
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WhenToldTo(x => x.SelectOne<int>(Param<DoesInstanceExistStatement>.Matches(y => y.InstanceId == InstanceId))).Return(0);
        };

        private Because of = () =>
        {
            InstanceExistsResponse = X2DataManager.DoesInstanceIdExist(InstanceId);
        };

        private It should_check_if_the_instance_exits = () =>
        {
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.SelectOne(Arg.Is<DoesInstanceExistStatement>(y => y.InstanceId == InstanceId)));
        };

        private It should_return_that_the_false = () =>
        {
            InstanceExistsResponse.ShouldBeFalse();
        };
    }
}