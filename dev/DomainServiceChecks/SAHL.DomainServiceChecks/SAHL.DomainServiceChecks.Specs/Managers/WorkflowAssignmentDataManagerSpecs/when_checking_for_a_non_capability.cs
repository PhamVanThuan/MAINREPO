using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.DomainServiceChecks.Managers.WorkflowAssignmentDataManagerSpecs;

namespace SAHL.DomainServiceCheck.Specs.Managers.WorkflowAssignmentDataManagerSpecs
{
    public class when_checking_for_a_non_capability : WithCoreFakes
    {
        private static IWorkflowAssignmentDataManager workflowAssignmentDataManager;
        private static int capability;
        private static bool InstanceExistsResponse;
        private static FakeDbFactory fakeDbFactory;

        private Establish context = () =>
        {
            fakeDbFactory = new FakeDbFactory();
            capability = 1;
            workflowAssignmentDataManager = new WorkflowAssignmentDataManager(fakeDbFactory);
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WhenToldTo(x => x.SelectWhere<CapabilityDataModel>
                ("[CapabilityKey] = 1", null)).Return(new List<CapabilityDataModel>());
        };

        private Because of = () =>
        {
            InstanceExistsResponse = workflowAssignmentDataManager.DoesCapabilityExist(capability);
        };

        private It should_check_if_the_capability_exits = () =>
        {
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.SelectWhere<CapabilityDataModel>
                ("[CapabilityKey] = 1", null));
        };

        private It should_return_false = () =>
        {
            InstanceExistsResponse.ShouldBeFalse();
        };
    }
}