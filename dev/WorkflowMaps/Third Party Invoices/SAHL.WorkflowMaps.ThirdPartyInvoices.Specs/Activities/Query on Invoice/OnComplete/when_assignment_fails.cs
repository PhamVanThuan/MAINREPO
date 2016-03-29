using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.Workflow.Shared;
using SAHL.Workflow.ThirdPartyInvoices;

namespace SAHL.WorkflowMaps.ThirdPartyInvoices.Specs.Activities.Query_on_Invoice.OnComplete
{
    [Subject("Activity => Query_on_Invoice => OnComplete")] // AutoGenerated
    internal class when_assignment_fails : WorkflowSpecThirdPartyInvoices
    {
        private static bool result;
        private static IWorkflowAssignment workflowAssignment;
        private static IThirdPartyInvoiceWorkflowProcess thirdPartyInvoices;

        private Establish context = () =>
        {
            workflowAssignment = An<IWorkflowAssignment>();
            thirdPartyInvoices = An<IThirdPartyInvoiceWorkflowProcess>();
            workflowAssignment.WhenToldTo(x => x.ReturnCaseToLastUserInCapability(messages, Param.IsAny<GenericKeyType>(), Param.IsAny<int>(), Param.IsAny<long>(),
              Param.IsAny<Capability>(), Param.IsAny<IServiceRequestMetadata>())).Return(false);
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(workflowAssignment);
            domainServiceLoader.RegisterMockForType<IThirdPartyInvoiceWorkflowProcess>(thirdPartyInvoices);
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Query_on_Invoice(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_use_the_metadata_from_the_params_when_performing_assignment = () =>
        {
            workflowAssignment.WhenToldTo(x => x.ReturnCaseToLastUserInCapability(messages, Param.IsAny<GenericKeyType>(), Param.IsAny<int>(), Param.IsAny<long>(),
              Param.IsAny<Capability>(), paramsData.ServiceRequestMetadata));
        };

        private It should_use_the_third_party_invoice_key_when_performing_assignment = () =>
        {
            workflowAssignment.WhenToldTo(x => x.ReturnCaseToLastUserInCapability(messages, GenericKeyType.ThirdPartyInvoice, workflowData.ThirdPartyInvoiceKey,
                instanceData.ID, Param.IsAny<Capability>(), paramsData.ServiceRequestMetadata));
        };

        private It should_assign_back_to_the_invoice_processor_capability = () =>
        {
            workflowAssignment.WhenToldTo(x => x.ReturnCaseToLastUserInCapability(messages, GenericKeyType.ThirdPartyInvoice, workflowData.ThirdPartyInvoiceKey,
                instanceData.ID, Capability.InvoiceProcessor, paramsData.ServiceRequestMetadata));
        };

        private It should_not_perform_the_query_command = () =>
        {
            thirdPartyInvoices.WasNotToldTo(x => x.QueryThirdPartyInvoice(messages, Param.IsAny<int>(), Param.IsAny<string>(), paramsData.ServiceRequestMetadata));
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}