using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Workflow.Shared;
using SAHL.WorkflowMaps.Specs.Common;

namespace SAHL.WorkflowMaps.ThirdPartyInvoices.Specs.Activities.Reassign_Processor.OnComplete
{
    [Subject("Activity => Reassign_Processor => OnComplete")]
    internal class when_reassign_processor_succeeds : WorkflowSpecThirdPartyInvoices
    {
        private static bool result;
        private static IWorkflowAssignment workflowAssignment;
        private static int userOrgStructureKey;

        private Establish context = () =>
        {
            userOrgStructureKey = 12345;
            ((ParamsDataStub)paramsData).Data = userOrgStructureKey;
            workflowAssignment = An<IWorkflowAssignment>();
            workflowAssignment.WhenToldTo(x => x.AssignCaseToSpecificUserInCapability(Param.IsAny<ISystemMessageCollection>(), GenericKeyType.ThirdPartyInvoice, Param.IsAny<int>(),
                Param.IsAny<long>(), Capability.InvoiceProcessor, Param.IsAny<IServiceRequestMetadata>(), Param.IsAny<int>())).Return(true);
            domainServiceLoader.RegisterMockForType(workflowAssignment);
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Reassign_Processor(instanceData, workflowData, paramsData, messages, ref message);
        };

        public It should_reassign_to_the_invoice_processor_capability = () =>
        {
            workflowAssignment.WasToldTo(x => x.AssignCaseToSpecificUserInCapability(Param.IsAny<ISystemMessageCollection>(), GenericKeyType.ThirdPartyInvoice, Param.IsAny<int>(),
                Param.IsAny<long>(), Capability.InvoiceProcessor, Param.IsAny<IServiceRequestMetadata>(), userOrgStructureKey));
        };

        public It should_use_the_genericKey_from_the_x2data = () =>
        {
            workflowAssignment.WasToldTo(x => x.AssignCaseToSpecificUserInCapability(Param.IsAny<ISystemMessageCollection>(), GenericKeyType.ThirdPartyInvoice, workflowData.GenericKey,
                Param.IsAny<long>(), Capability.InvoiceProcessor, Param.IsAny<IServiceRequestMetadata>(), userOrgStructureKey));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}