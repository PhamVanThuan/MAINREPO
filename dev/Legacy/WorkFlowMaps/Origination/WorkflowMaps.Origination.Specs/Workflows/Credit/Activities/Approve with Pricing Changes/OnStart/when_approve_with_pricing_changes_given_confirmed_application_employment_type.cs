using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.Credit.Specs;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Approve_with_Pricing_Changes.OnStart
{
    [Subject("Activity => Approve_With_Pricing_Changes => OnStart")]
    internal class when_approve_with_pricing_changes_given_confirmed_application_employment_type : WorkflowSpecCredit
    {
        private static bool result;
        private static ICredit credit;

        private Establish context = () =>
        {
            result = false;
            credit = An<ICredit>();
            credit.WhenToldTo(c => c.CheckEmploymentTypeConfirmedRule((IDomainMessageCollection)messages, instanceData.ID, false)).Return(true);
            domainServiceLoader.RegisterMockForType<ICredit>(credit);
            workflowData.ActionSource = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Approve_with_Pricing_Changes(instanceData, workflowData, paramsData, messages);
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