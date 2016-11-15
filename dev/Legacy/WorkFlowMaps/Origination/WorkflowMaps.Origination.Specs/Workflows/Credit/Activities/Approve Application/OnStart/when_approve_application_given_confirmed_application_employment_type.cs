using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Credit.Specs.Activities.Approve_Application.OnStart
{
    [Subject("Activity => Approve_Application => OnStart")] // AutoGenerated
    internal class when_approve_application_given_confirmed_application_employment_type : WorkflowSpecCredit
    {
        private static bool result;
        private static ICredit credit;

        private Establish context = () =>
        {
            result = false;
            credit = An<ICredit>();
            credit.WhenToldTo(c => c.CheckEmploymentTypeConfirmedRule((IDomainMessageCollection)messages, instanceData.ID, false)).Return(true);
            domainServiceLoader.RegisterMockForType<ICredit>(credit);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Approve_Application(instanceData, workflowData, paramsData, messages);
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