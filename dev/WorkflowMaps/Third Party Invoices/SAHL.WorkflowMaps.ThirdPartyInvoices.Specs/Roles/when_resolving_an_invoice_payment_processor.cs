using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Workflow.Shared;

namespace SAHL.WorkflowMaps.ThirdPartyInvoices.Specs.Roles
{
    public class when_resolving__an_invoice_payment_processor : WorkflowSpecThirdPartyInvoices
    {
        private static string result;
        private static IWorkflowAssignment workflowAssignmentCommon;
        private static string expectedUser;

        private Establish context = () =>
         {
             expectedUser = @"SAHL\PaymentProcessor";
             workflowAssignmentCommon = An<IWorkflowAssignment>();
             workflowAssignmentCommon.WhenToldTo((x => x.ResolveUserInCapability(messages, instanceData.ID, Capability.InvoicePaymentProcessor)))
               .Return(expectedUser);
             domainServiceLoader.RegisterMockForType(workflowAssignmentCommon);
         };

        private Because of = () =>
         {
             result = workflow.OnGetRole_Third_Party_Invoices_Invoice_Payment_Processor_D(instanceData, workflowData, "Invoice Payment Processor D", paramsData, messages);
         };

        private It should_return_the_result_from_the_workflow_assignment_service = () =>
         {
             result.ShouldEqual(expectedUser);
         };

        private It should_resolve_the_correct_role = () =>
         {
             workflowAssignmentCommon.WasToldTo((x => x.ResolveUserInCapability(messages, instanceData.ID, Capability.InvoicePaymentProcessor)));
         };
    }
}