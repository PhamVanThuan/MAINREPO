using Machine.Specifications;
using Machine.Fakes;
using WorkflowMaps.Credit.Specs;
using X2DomainService.Interface.Origination;
using SAHL.Common.Collections.Interfaces;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Exceptions_Rate_Adjustment.OnStart
{
    [Subject("Activity => Exceptions_Rate_Adjustment => OnStart")]
    internal class when_exceptions_rate_adjustment_given_unconfirmed_employment : WorkflowSpecCredit
    {
        private static bool result;
        private static ICredit credit;

        Establish context = () =>
        {
            result = true;
            workflowData.ActionSource = string.Empty;
            workflowData.ExceptionsDeclineWithOffer = false;
            credit = An<ICredit>();
            credit.WhenToldTo(c => c.CheckEmploymentTypeConfirmedRule((IDomainMessageCollection)messages, instanceData.ID, false)).Return(false);
            domainServiceLoader.RegisterMockForType<ICredit>(credit);
        };

        Because of = () =>
        {
            result = workflow.OnStartActivity_Exceptions_Rate_Adjustment(instanceData, workflowData, paramsData, messages);
        };

        It should_check_employement_Type_confirmed_rules = () =>
        {
            credit.WasToldTo(c => c.CheckEmploymentTypeConfirmedRule((IDomainMessageCollection)messages, instanceData.ID, false));
        };

        It should_not_set_action_source_data_property = () =>
        {
            workflowData.ActionSource.ShouldBeEmpty();
        };

        It should_not_set_the_exceptions_decline_with_offer_data_property = () =>
        {
            workflowData.ExceptionsDeclineWithOffer.ShouldBeFalse();
        };

        It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };				
    }
}
