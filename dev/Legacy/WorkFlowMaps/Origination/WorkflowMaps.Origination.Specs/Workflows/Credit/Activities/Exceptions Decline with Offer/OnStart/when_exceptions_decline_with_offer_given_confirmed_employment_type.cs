using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Credit.Specs.Activities.Exceptions_Decline_with_Offer.OnStart
{
    [Subject("Activity => Exceptions_Decline_with_Offer => OnStart")]
    internal class when_exceptions_decline_with_offer_given_confirmed_employment_type : WorkflowSpecCredit
    {
        private static bool result;
        private static ICredit credit;

        private Establish context = () =>
        {
            result = false;
            workflowData.ActionSource = string.Empty;
            credit = An<ICredit>();
            credit.WhenToldTo(c => c.CheckEmploymentTypeConfirmedRule((IDomainMessageCollection)messages, instanceData.ID, false)).Return(true);
            domainServiceLoader.RegisterMockForType<ICredit>(credit);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Exceptions_Decline_with_Offer(instanceData, workflowData, paramsData, messages);
        };

        private It should_ensure_that_application_employment_type_has_been_confirmed = () =>
        {
            credit.WasToldTo(c => c.CheckEmploymentTypeConfirmedRule((IDomainMessageCollection)messages, instanceData.ID, false));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}