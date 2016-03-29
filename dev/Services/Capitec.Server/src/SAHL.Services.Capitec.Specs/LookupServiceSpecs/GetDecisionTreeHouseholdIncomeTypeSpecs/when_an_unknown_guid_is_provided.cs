using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.Capitec.Managers.Lookup;
using System;

namespace SAHL.Services.Capitec.Specs.LookupServiceSpecs.GetDecisionTreeHouseholdIncomeTypeSpecs
{
    public class when_an_unknown_guid_is_provided : WithFakes
    {
        private static ILookupManager lookupService;
        private static FakeDbFactory dbFactory;
        private static Guid employmentType;   
        private static Exception exception;

        private Establish context = () =>
            {
                employmentType = Guid.NewGuid();
                dbFactory = new FakeDbFactory();
                lookupService = new LookupManager(dbFactory);
            };

        private Because of = () =>
            {
                exception = Catch.Exception(() => lookupService.GetDecisionTreeHouseholdIncomeType(employmentType));
            };

        private It should_throw_a_notsupported_exception = () =>
            {
                exception.ShouldBeOfExactType<NotSupportedException>();
            };
    }
}
