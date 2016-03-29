using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Commands;

namespace SAHL.Services.Interfaces.WorkflowAssignmentDomain.Specs.Commands
{
    public class when_creating_an_assign_workflow_case_command : WithFakes
    {
        private static AssignWorkflowCaseCommand result;
        private static long instanceId;
        private static int userOrganisationStructureKey;
        private static Capability capability;
        private static GenericKeyType genericKeyType;
        private static int genericKey;

        private Establish that = () =>
        {
            capability = Capability.InvoiceProcessor;
            genericKey = 2;
            instanceId = 3L;
            userOrganisationStructureKey = 4;
            genericKeyType = GenericKeyType.ThirdPartyInvoice;
        };

        private Because of = () =>
        {
            result = new AssignWorkflowCaseCommand(genericKeyType, genericKey, instanceId, userOrganisationStructureKey, capability);
        };

        private It should_have_the_expected_matching_fields = () =>
        {
            new object[]
            {
                genericKeyType,
                genericKey,
                instanceId,
                userOrganisationStructureKey,
                capability,
            }.ShouldEqual(new object[]
            {
                result.GenericKeyTypeKey,
                result.GenericKey,
                result.InstanceId,
                result.UserOrganisationStructureKey,
                result.Capability,
            });
        };
    }
}