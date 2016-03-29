using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.ApplicationCapture.Specs.States.Assign_TeleConsultant.OnEnter
{
    [Subject("State => Assign_TeleConsultant => OnExit")]
    internal class when_assign_teleconsultant : WorkflowSpecApplicationCapture
    {
        private static bool result;
        private static IWorkflowAssignment assign;

        private Establish context = () =>
        {
            result = false;
            instanceData.Name = "Test";
            workflowData.IsEA = true;
            workflowData.ApplicationKey = 1;
            assign = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assign);
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Assign_TeleConsultant(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_name_property = () =>
        {
            instanceData.Name.ShouldMatch(workflowData.ApplicationKey.ToString());
        };

        private It should_set_ea_property_to_false = () =>
        {
            workflowData.IsEA.ShouldBeFalse();
        };

        private It should_insert_internet_lead = () =>
        {
            assign.WasToldTo(x => x.InsertInternetLeadWorkflowAssignment((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey, "Assign TeleConsultant"));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}