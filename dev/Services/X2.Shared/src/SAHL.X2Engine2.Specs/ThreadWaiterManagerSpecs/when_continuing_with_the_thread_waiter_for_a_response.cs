using System;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.X2.Messages;

namespace SAHL.X2Engine2.Specs.ThreadWaiterManagerSpecs
{
    public class when_continuing_with_the_thread_waiter_for_a_response : WithFakes
    {
        private static IResponseThreadWaiter responseThreadWaiter;
        private static X2Response response;
        private static Guid correlationId = Guid.NewGuid();
        Establish context = () =>
        {
            response = new X2Response(correlationId, "Message", 1234567L);
            responseThreadWaiter = new ResponseThreadWaiter(correlationId);
        };

        Because of = () =>
        {
            responseThreadWaiter.Continue(response);
        };

        It should_set_the_response_for_the_thread_waiter_to_the_provided_response = () =>
        {
            responseThreadWaiter.Response.ShouldBeTheSameAs(response);
        };

        It should_having_matching_correlation_id = () =>
        {
            responseThreadWaiter.CorrelationId.ShouldEqual(response.RequestID);
        };
    }
}