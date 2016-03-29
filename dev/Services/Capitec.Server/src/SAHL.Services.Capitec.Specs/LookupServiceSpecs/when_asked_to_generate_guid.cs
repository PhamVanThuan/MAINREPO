using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.Capitec.Managers.Lookup;
using System;
using System.Linq;

namespace SAHL.Services.Capitec.Specs.LookupServiceSpecs
{
    public class when_asked_to_generate_guid : WithFakes
    {
        private static ILookupManager lookupService;
        private static FakeDbFactory dbFactory;
        private static Guid newGuid;

        private Establish context = () =>
            {
                dbFactory = new FakeDbFactory();
                lookupService = new LookupManager(dbFactory);
            };

        private Because of = () =>
            {
                newGuid = lookupService.GenerateCombGuid();
            };

        private It should_return_a_guid = () =>
            {
                newGuid.ShouldNotBeNull();
                newGuid.ShouldNotEqual<Guid>(new System.Guid());
            };
    }
}