using Machine.Fakes;
using Machine.Specifications;
using SAHL.Workflow.Shared;
using SAHL.Core.BusinessModel.Enums;

namespace SAHL.WorkflowMaps.ThirdPartyInvoices.Specs.Roles
{
    public class when_resolving_an_invoice_processor : WorkflowSpecThirdPartyInvoices
    {
        private static string result;
        private static IWorkflowAssignment workflowAssignmentCommon;
        private static string expectedUser;

        Establish context = () =>
        {
            expectedUser = @"SAHL\InvoiceProcessor";
            workflowAssignmentCommon = An<IWorkflowAssignment>();
            workflowAssignmentCommon.WhenToldTo((x=>x.ResolveUserInCapability(messages, instanceData.ID, Capability.InvoiceProcessor)))
              .Return(expectedUser);
            domainServiceLoader.RegisterMockForType(workflowAssignmentCommon);
        };

        Because of = () =>
        {
            result = workflow.OnGetRole_Third_Party_Invoices_Invoice_Processor_D(instanceData, workflowData, "Invoice Processor D", paramsData, messages);
        };

        It should_return_the_result_from_the_workflow_assignment_service = () =>
        {
            result.ShouldEqual(expectedUser);
        };

        private It should_resolve_the_correct_role = () =>
        {
            workflowAssignmentCommon.WasToldTo((x => x.ResolveUserInCapability(messages, instanceData.ID, Capability.InvoiceProcessor)));
        };
    }
}
