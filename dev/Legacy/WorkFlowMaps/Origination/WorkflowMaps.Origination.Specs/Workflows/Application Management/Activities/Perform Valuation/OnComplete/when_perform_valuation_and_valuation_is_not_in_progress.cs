using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Common;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Perform_Valuation.OnComplete
{
    [Subject("Activity => Perform_Valuation => OnComplete")]
    internal class when_perform_valuation_and_valuation_is_not_in_progress : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string message;
        private static ICommon common;

        private Establish context = () =>
        {
            result = true;
            message = string.Empty;
            ((InstanceDataStub)instanceData).ID = 1;
            workflowData.ApplicationKey = 2;
            common = An<ICommon>();
            common.WhenToldTo(x => x.IsValuationInProgress(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<long>(), Param.IsAny<int>()))
                .Return(false);
            domainServiceLoader.RegisterMockForType<ICommon>(common);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Perform_Valuation(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_check_if_valuation_is_in_progress = () =>
        {
            common.WasToldTo(x => x.IsValuationInProgress((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}