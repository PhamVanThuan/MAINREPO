using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Commands;
using SAHL.Services.WorkflowAssignmentDomain.Managers;
using SAHL.Services.WorkflowAssignmentDomain.Managers.Statements;

namespace SAHL.Services.WorkflowAssignmentDomain.Specs.Managers
{
    public class when_assigning_a_workflow_case : WithFakes
    {
        private Establish that = () =>
        {
            command = new AssignWorkflowCaseCommand(GenericKeyType.ThirdPartyInvoice, 2, 3L, 4, Capability.InvoiceApprover);

            dbFactory = An<FakeDbFactory>();

            dataManager = new WorkflowCaseDataManager(dbFactory);
        };

        private Because of = () =>
        {
            dataManager.AssignWorkflowCase(command);
        };

        private It should_have_executed_the_statement = () =>
        {
            dbFactory
                .NewDb()
                .InAppContext()
                .WasToldTo(a => a.ExecuteNonQuery(Param<AssignWorkflowCaseStatement>
                    .Matches(b => b.CapabilityKey == (int)command.Capability
                        && b.InstanceId == command.InstanceId
                        && b.UserOrganisationStructureKey == command.UserOrganisationStructureKey)))
                .OnlyOnce();
        };

        private It should_have_performed_a_complete_on_the_context = () =>
        {
            dbFactory.NewDb().InAppContext().WasToldTo(a => a.Complete()).OnlyOnce();
        };

        private static AssignWorkflowCaseCommand command;
        private static FakeDbFactory dbFactory;
        private static WorkflowCaseDataManager dataManager;
    }
}