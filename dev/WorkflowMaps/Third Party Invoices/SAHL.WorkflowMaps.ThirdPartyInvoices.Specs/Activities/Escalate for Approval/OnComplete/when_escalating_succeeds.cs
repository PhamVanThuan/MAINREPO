using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Workflow.Shared;
using SAHL.Workflow.ThirdPartyInvoices;
using SAHL.WorkflowMaps.Specs.Common;

namespace SAHL.WorkflowMaps.ThirdPartyInvoices.Specs.Activities.Escalate_for_Approval.OnComplete
{
    [Subject("Activity => Escalate_for_Approval => OnComplete")]
    internal class when_escalating_succeeds : WorkflowSpecThirdPartyInvoices
    {
        private static bool result;
        private static IWorkflowAssignment workflowAssignment;
        private static int userOrgStructureKey;
        private static IThirdPartyInvoiceWorkflowProcess thirdPartyInvoices;


        private Establish context = () =>
        {
            userOrgStructureKey = 12345;
            ((ParamsDataStub)paramsData).Data = userOrgStructureKey;
            workflowAssignment = An<IWorkflowAssignment>();
            thirdPartyInvoices = An<IThirdPartyInvoiceWorkflowProcess>();
            workflowAssignment.WhenToldTo(x => x.AssignCaseToSpecificUserInCapability(Param.IsAny<ISystemMessageCollection>(), GenericKeyType.ThirdPartyInvoice, Param.IsAny<int>(),
                Param.IsAny<long>(), Capability.InvoiceApprover, Param.IsAny<IServiceRequestMetadata>(), Param.IsAny<int>())).Return(true);
            thirdPartyInvoices.WhenToldTo(x => x.EscalateThirdPartyInvoiceForApproval(Param.IsAny<ISystemMessageCollection>(), Param.IsAny<int>(), Param.IsAny<int>(),
                Param.IsAny<IServiceRequestMetadata>())).Return(true);
            domainServiceLoader.RegisterMockForType(workflowAssignment);
            domainServiceLoader.RegisterMockForType(thirdPartyInvoices);
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Escalate_for_Approval(instanceData, workflowData, paramsData, messages, ref message);
        };

        public It should_reassign_to_the_invoice_processor_capability = () =>
        {
            workflowAssignment.WasToldTo(x => x.AssignCaseToSpecificUserInCapability(Param.IsAny<ISystemMessageCollection>(), GenericKeyType.ThirdPartyInvoice, Param.IsAny<int>(),
                Param.IsAny<long>(), Capability.InvoiceApprover, Param.IsAny<IServiceRequestMetadata>(), userOrgStructureKey));
        };

        public It should_use_the_genericKey_from_the_x2data = () =>
        {
            workflowAssignment.WasToldTo(x => x.AssignCaseToSpecificUserInCapability(Param.IsAny<ISystemMessageCollection>(), GenericKeyType.ThirdPartyInvoice, workflowData.GenericKey,
                Param.IsAny<long>(), Capability.InvoiceApprover, Param.IsAny<IServiceRequestMetadata>(), userOrgStructureKey));
        };

        public It should_call_the_escalate_domain_command = () =>
        {
            thirdPartyInvoices.WasToldTo(x => x.EscalateThirdPartyInvoiceForApproval(Param.IsAny<ISystemMessageCollection>(), workflowData.ThirdPartyInvoiceKey,
                userOrgStructureKey, Param.IsAny<IServiceRequestMetadata>()));
        };
        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}