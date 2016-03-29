using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System;
using X2DomainService.Interface.DebtCounselling;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Term_expired.OnGetActivityTime
{
    [Subject("Activity => Term_expired => OnGetActivityTime")]
    internal class when_getting_activity_time : WorkflowSpecDebtCounselling
    {
        private static DateTime activityTime;
        private static DateTime reviewDate;
        private static IDebtCounselling client;

        private Establish context = () =>
        {
            reviewDate = new DateTime(2012, 1, 1);
            client = An<IDebtCounselling>();
            domainServiceLoader.RegisterMockForType<IDebtCounselling>(client);
            client.WhenToldTo(x => x.GetReviewDate(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>())).Return(reviewDate);
        };

        private Because of = () =>
        {
            activityTime = workflow.GetActivityTime_Term_expired(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_the_review_date = () =>
        {
            activityTime.ShouldEqual(reviewDate);
        };
    }
}