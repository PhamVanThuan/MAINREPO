using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.Workflow.Shared;
using SAHL.Workflow.ThirdPartyInvoices;
using SAHL.WorkflowMaps.Specs.Common;

namespace SAHL.WorkflowMaps.ThirdPartyInvoices.Specs.Activities.Query_on_Invoice.OnComplete
{
    public class when_param_data_is_null : WorkflowSpecThirdPartyInvoices
    {
        private static bool result;
        private static IWorkflowAssignment workflowAssignment;
        private static IThirdPartyInvoiceWorkflowProcess thirdPartyInvoices;

        private Establish context = () =>
        {
            ((ParamsDataStub)paramsData).Data = null;
            workflowAssignment = An<IWorkflowAssignment>();
            thirdPartyInvoices = An<IThirdPartyInvoiceWorkflowProcess>();
            workflowAssignment.WhenToldTo(x => x.ReturnCaseToLastUserInCapability(messages, Param.IsAny<GenericKeyType>(), Param.IsAny<int>(), Param.IsAny<long>(),
              Param.IsAny<Capability>(), Param.IsAny<IServiceRequestMetadata>())).Return(true);
            thirdPartyInvoices.WhenToldTo(x => x.QueryThirdPartyInvoice(messages, Param.IsAny<int>(), string.Empty, Param.IsAny<IServiceRequestMetadata>()))
                .Return(true);
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(workflowAssignment);
            domainServiceLoader.RegisterMockForType<IThirdPartyInvoiceWorkflowProcess>(thirdPartyInvoices);
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Query_on_Invoice(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}