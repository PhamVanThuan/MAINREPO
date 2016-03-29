using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FinanceDomain.Managers;
using SAHL.Services.FinanceDomain.Managers.Capability;
using SAHL.Services.FinanceDomain.Managers.Capability.Statements;
using System.Collections.Generic;

namespace SAHL.Services.FinanceDomain.Specs.Managers.CapabilityManagerSpec
{
    public class when_retrieving_capability_mandates : WithFakes
    {
        static ICapabilityManager capabilityManager;
        static FakeDbFactory fakeDb;
        static IEnumerable<ApprovalMandateRanges> mandates;
        static IEnumerable<ApprovalMandateRanges> expectedMandates;

        Establish context = () =>
        {
            fakeDb = new FakeDbFactory();
            capabilityManager = new CapabilityManager(fakeDb);

            expectedMandates = new List<ApprovalMandateRanges> { 
                new ApprovalMandateRanges { Capability = "Invoice Approver up to R50000", LowerBound = 0.00M, UpperBound = 200.00M } 
            };

            fakeDb.NewDb().InReadOnlyAppContext().WhenToldTo(x => x.Select(Param.IsAny<GetCapabilityMandatesStatement>())).Return(expectedMandates);
        };

        Because of = () =>
        {
            mandates = capabilityManager.GetCapabilityMandates();
        };

        It should_get_capabalities_from_the_database = () =>
        {
            fakeDb.NewDb().InReadOnlyAppContext().WasToldTo(x => x.Select(Param.IsAny<GetCapabilityMandatesStatement>()));
        };

        It should_return_the_capability_mandates = () =>
        {
            mandates.ShouldEqual(expectedMandates);
        };
    }
}
