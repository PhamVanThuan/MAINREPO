using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.Credit.Specs;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Exceptions_Rate_Adjustment.OnStart
{
    [Subject("Activity => Exceptions_Rate_Adjustment => OnStart")]
    internal class when_exceptions_rate_adjustment_given_confirmed_employment : WorkflowSpecCredit
    {
        private static bool result;
        private static ICredit credit;

        private Establish context = () =>
        {
            result = false;
            workflowData.ActionSource = string.Empty;
            workflowData.ExceptionsDeclineWithOffer = false;
            credit = An<ICredit>();
            credit.WhenToldTo(c => c.CheckEmploymentTypeConfirmedRule((IDomainMessageCollection)messages, instanceData.ID, false)).Return(true);
            domainServiceLoader.RegisterMockForType<ICredit>(credit);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Exceptions_Rate_Adjustment(instanceData, workflowData, paramsData, messages);
        };

        private It should_check_employement_Type_confirmed_rules = () =>
        {
            credit.WasToldTo(c => c.CheckEmploymentTypeConfirmedRule((IDomainMessageCollection)messages, instanceData.ID, false));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}