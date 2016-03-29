using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Credit.Specs.Activities.Exceptions_Decline_with_Offer.OnStart
{
    [Subject("Activity => Exceptions_Decline_with_Offer => OnStart")] 
    internal class when_exceptions_decline_with_offer_given_unconfirmed_employment_type : WorkflowSpecCredit
    {
        private static bool result;
        private static ICredit credit;

        private Establish context = () =>
        {
            result = true;
            workflowData.ActionSource = string.Empty;
            credit = An<ICredit>();
            credit.WhenToldTo(c => c.CheckEmploymentTypeConfirmedRule((IDomainMessageCollection)messages, instanceData.ID, false)).Return(false);
            domainServiceLoader.RegisterMockForType<ICredit>(credit);
        };

        Because of = () =>
        {
            result = workflow.OnStartActivity_Exceptions_Decline_with_Offer(instanceData, workflowData, paramsData, messages);
        };

        private It should_check_employement_Type_confirmed_rules = () =>
        {
            credit.WasToldTo(c => c.CheckEmploymentTypeConfirmedRule((IDomainMessageCollection)messages, instanceData.ID, false));
        };

        private It should_not_set_action_source_data_property = () =>
        {
            workflowData.ActionSource.ShouldBeEmpty();
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}