using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationCapture.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Decline.OnComplete
{
    [Subject("Activity => Decline => OnComplete")]
    internal class when_decline : WorkflowSpecApplicationCapture
    {
        private static bool result;
        private static bool expectedResult;
        private static string message;
        private static IApplicationCapture appCap;

        private Establish context = () =>
        {
            result = false;
            expectedResult = true;
            message = string.Empty;
            workflowData.ApplicationKey = 1;
            ((ParamsDataStub)paramsData).IgnoreWarning = true;
            appCap = An<IApplicationCapture>();
            appCap.WhenToldTo(x => x.DemoteMainApplicantToLead(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>()))
                .Return(expectedResult);
            domainServiceLoader.RegisterMockForType<IApplicationCapture>(appCap);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Decline(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_demote_main_applicant_to_lead = () =>
        {
            appCap.WasToldTo(x => x.DemoteMainApplicantToLead((IDomainMessageCollection)messages, workflowData.ApplicationKey, paramsData.IgnoreWarning));
        };

        private It should_return_demote_main_applicant_to_lead_result = () =>
        {
            result.ShouldEqual(expectedResult);
        };
    }
}