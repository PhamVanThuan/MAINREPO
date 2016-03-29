using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Valuations.Specs.Activities.Review_Valuation_Requested.OnComplete
{
    [Subject("Activity => Review_Valuation_Requested => OnComplete")]
    internal class when_review_request_with_lightstone_valuation_data : WorkflowSpecValuations
    {
        private static bool result;
        private static string message;
        private static ICommon client;
        private static IWorkflowAssignment assignmentClient;
        private static int expectedApplicationKey;
        private static int expectedNumberOfLoops;
        private static string expectedSubject;
        private static IValuations valuationsClient;
        private static Dictionary<string, object> expectedData;

        private Establish context = () =>
        {
            expectedData = new Dictionary<string, object>();
            expectedData.Add("LightstonePropertyID", "99999");
            expectedData.Add("ValuationKey", 123);
            expectedData.Add("PropertyKey", 321);
            expectedData.Add("ValuationDataProviderDataServiceKey", 2);

            client = An<ICommon>();
            assignmentClient = An<IWorkflowAssignment>();
            valuationsClient = An<IValuations>();
            domainServiceLoader.RegisterMockForType<ICommon>(client);
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignmentClient);
            domainServiceLoader.RegisterMockForType<IValuations>(valuationsClient);

            valuationsClient.WhenToldTo(x => x.GetValuationData((IDomainMessageCollection)messages, expectedApplicationKey)).Return(expectedData);
        };

        private Because of = () =>
        {
            workflow.OnCompleteActivity_Review_Valuation_Requested(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_set_lightstonepropertyid_data_property_to_lightstonepropertyid_dictionary_entry = () =>
        {
            workflowData.LightstonePropertyID.ShouldEqual<string>((string)expectedData["LightstonePropertyID"]);
        };
    }
}