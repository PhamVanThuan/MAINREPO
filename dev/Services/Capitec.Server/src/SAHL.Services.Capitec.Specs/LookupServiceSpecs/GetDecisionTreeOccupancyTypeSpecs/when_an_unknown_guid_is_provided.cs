using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.Capitec.Managers.Lookup;
using System;

namespace SAHL.Services.Capitec.Specs.LookupServiceSpecs.GetDecisionTreeOccupancyTypeSpecs
{
    public class when_an_unknown_guid_is_provided : WithFakes
    {
        private static ILookupManager lookupService;
        private static FakeDbFactory dbFactory;
        private static Guid occupancyType;
        private static string result;
        private static Exception exception;

        private Establish context = () =>
        {
            occupancyType = Guid.NewGuid();
            dbFactory = new FakeDbFactory();
            lookupService = new LookupManager(dbFactory);
        };

        private Because of = () =>
        {
            exception = Catch.Exception(() => lookupService.GetDecisionTreeOccupancyType(occupancyType));
        };

        private It should_throw_a_notsupported_exception = () =>
        {
            exception.ShouldBeOfExactType<NotSupportedException>();
        };
    }
}