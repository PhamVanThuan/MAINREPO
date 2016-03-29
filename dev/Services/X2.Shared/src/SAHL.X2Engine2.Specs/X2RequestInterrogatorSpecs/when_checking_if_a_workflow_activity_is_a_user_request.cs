using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.X2.Messages;
using StructureMap.AutoMocking;
using System;

namespace SAHL.X2Engine2.Specs.X2RequestInterrogatorSpecs
{
    public class when_checking_if_a_workflow_activity_is_a_user_request : WithFakes
    {
        private static AutoMocker<X2RequestInterrogator> autoMocker;
        private static IX2Request request;
        private static bool result = true, expectedResult = false;

        private Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2RequestInterrogator>();
            request = new X2Request(Guid.NewGuid(), X2RequestType.WorkflowActivity, null, false, null);
        };

        private Because of = () =>
        {
            result = autoMocker.ClassUnderTest.IsRequestMonitored(request);
        };

        private It should_return_the_workflow_for_the_given_request = () =>
        {
            result.ShouldEqual(expectedResult);
        };
    }
}