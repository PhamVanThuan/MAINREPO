using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.Internet_Lead.OnComplete
{
    [Subject("Activity => Internet_Lead => OnComplete")] // AutoGenerated
    public class when_internet_lead_returns_false : WorkflowSpecApplicationCapture
    {
        private static bool result;
        private static IApplicationCapture applicationCapture;

        private Establish context = () =>
        {
            result = true;
            applicationCapture = An<IApplicationCapture>();
            applicationCapture.WhenToldTo(x => x.CreateInternetLead(Param<IDomainMessageCollection>.IsAnything, Param<int>.IsAnything, Param<string>.IsAnything, Param<bool>.IsAnything)).Return(false);
            domainServiceLoader.RegisterMockForType<IApplicationCapture>(applicationCapture);
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Internet_Lead(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}