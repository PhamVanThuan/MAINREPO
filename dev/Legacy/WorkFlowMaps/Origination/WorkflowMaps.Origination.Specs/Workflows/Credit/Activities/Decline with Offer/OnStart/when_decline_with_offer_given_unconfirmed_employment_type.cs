using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.Credit.Specs;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Decline_with_Offer.OnStart
{
    [Subject("Activity => Decline_With_Offer => OnStart")]
    internal class when_decline_with_offer_given_unconfirmed_employment_type : WorkflowSpecCredit
    {
        private static bool result;
        private static ICredit credit;

        private Establish context = () =>
        {
            result = false;
            workflowData.ActionSource = string.Empty;
            credit = An<ICredit>();
            credit.WhenToldTo(c => c.CheckEmploymentTypeConfirmedRule((IDomainMessageCollection)messages, instanceData.ID, false)).Return(false);
            domainServiceLoader.RegisterMockForType<ICredit>(credit);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Decline_with_Offer(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}