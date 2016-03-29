using Machine.Specifications;
using Machine.Fakes;
using WorkflowMaps.Credit.Specs;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Exceptions_Rate_Adjustment.OnStart
{
    [Subject("Activity => Exceptions_Rate_Adjustment => OnStart")]
    internal class when_exceptions_rate_adjustment : WorkflowSpecCredit
    {
        private static bool result;
        private static ICredit credit;

        private Establish context = () =>
        {
            result = false;
            credit = An<ICredit>();
            workflowData.ExceptionsDeclineWithOffer = false;
            credit.WhenToldTo(c => c.CheckEmploymentTypeConfirmedRule(messages, instanceData.InstanceID, false)).Return(true);
            domainServiceLoader.RegisterMockForType<ICredit>(credit);
        };

        Because of = () =>
        {
            result = workflow.OnStartActivity_Exceptions_Rate_Adjustment(messages, workflowData, instanceData, paramsData);
        };

        It should_ensure_that_application_employment_type_has_been_confirmed = () =>
        {
            credit.WasToldTo(c => c.CheckEmploymentTypeConfirmedRule(messages, instanceData.InstanceID, false));
        };

        It should_set_the_esceptions_decline_with_offer_data_property = () =>
        {
            workflowData.ExceptionsDeclineWithOffer.ShouldBeTrue();
        };

        It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };				
    }
}
