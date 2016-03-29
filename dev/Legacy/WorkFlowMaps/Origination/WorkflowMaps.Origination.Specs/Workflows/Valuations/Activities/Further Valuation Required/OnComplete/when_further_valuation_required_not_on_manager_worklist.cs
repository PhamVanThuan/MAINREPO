using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System;
using WorkflowMaps.Valuations.Specs;
using X2DomainService.Interface.Common;

namespace WorkflowMaps.Origination.Specs.Workflows.Valuations.Activities.Further_Valuation_Required.OnComplete
{
    [Subject("Activity => Further_Valuation_Required => OnComplete")]
    internal class when_further_valuation_required_not_on_manager_worklist : WorkflowSpecValuations
    {
        private static ICommon client;
        private static string message;
        private static bool result;

        private Establish context = () =>
        {
            client = An<ICommon>();
            message = String.Empty;
            domainServiceLoader.RegisterMockForType<ICommon>(client);
            result = false;
            workflowData.OnManagerWorkList = false;
            client.WhenToldTo(x => x.IsValuationInProgress((IDomainMessageCollection)messages, 0, 0)).Return(true);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Further_Valuation_Required(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_check_if_valuation_in_progress = () =>
        {
            client.WasToldTo(x => x.IsValuationInProgress((IDomainMessageCollection)messages, 0, 0));
        };

        private It should_return_is_valuation_in_progress_return_value = () =>
        {
            result.ShouldBeTrue();
        };
    }
}