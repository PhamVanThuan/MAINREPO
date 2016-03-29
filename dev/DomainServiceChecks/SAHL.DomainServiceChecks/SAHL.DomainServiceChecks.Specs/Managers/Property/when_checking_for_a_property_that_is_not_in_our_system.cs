using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Testing.Fakes;
using SAHL.DomainServiceChecks.Managers.PropertyDataManager;
using SAHL.DomainServiceChecks.Managers.PropertyDataManager.Statements;

namespace SAHL.DomainService.Check.Specs.Managers.Client
{
    public class when_checking_for_a_property_that_is_not_in_our_system : WithFakes
    {
        private static PropertyDataManager propertyDataManager;
        private static int propertyKey;
        private static bool propertyExistsResponse;
        private static FakeDbFactory fakeDbFactory;

        private Establish context = () =>
        {
            fakeDbFactory = new FakeDbFactory();
            propertyKey = 1;
            propertyDataManager = new PropertyDataManager(fakeDbFactory);
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WhenToldTo(x => x.SelectOne<int>(Param.IsAny<PropertyExistsStatement>())).Return(0);
        };

        private Because of = () =>
        {
            propertyExistsResponse = propertyDataManager.IsPropertyOnOurSystem(propertyKey);
        };

        private It should_check_if_the_property_exists = () =>
        {
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.SelectOne(Arg.Is<PropertyExistsStatement>(y => y.PropertyKey == propertyKey)));
        };

        private It should_acknolewdge_that_the_property_exists_in_our_system = () =>
        {
            propertyExistsResponse.ShouldBeFalse();
        };
    }
}