using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.ApplicationCapture.Specs.States.Capitec_Applications.OnEnter
{
    [Subject("State => Capitec_Applications => OnEnter")]
    internal class when_capitec_applications : WorkflowSpecApplicationCapture
    {
        private static bool result;
        private static int applicationKey;
        private static string expectedInstanceName, expectedCaseName;
        private static string assignedTo;
        private static ICommon common;
        private static IWorkflowAssignment workflowAssignment;

        private Establish context = () =>
        {
            result = false;
            instanceData.Subject = string.Empty;
            instanceData.Name = string.Empty;
            instanceData.ID = 0;

            assignedTo = @"SAHL\TELCUser";
            applicationKey = 123456;
            expectedInstanceName = Convert.ToString(applicationKey);
            expectedCaseName = "Mr Test & Mrs Test";

            common = An<ICommon>();
            workflowAssignment = An<IWorkflowAssignment>();

            domainServiceLoader.RegisterMockForType<ICommon>(common);
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(workflowAssignment);

            workflowData.ApplicationKey = applicationKey; 
            common.WhenToldTo(x => x.GetCaseName(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>())).Return(expectedCaseName);

            workflowAssignment.WhenToldTo(x => x.ReactiveUserOrRoundRobinForOSKeyByProcess(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<string>(), Param.IsAny<int>(), Param.IsAny<int>(), Param.IsAny<long>(), Param.IsAny<string>(), Param.IsAny<SAHL.Common.Globals.Process>(), Param.IsAny<int>())).Return(assignedTo);

        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Capitec_Applications(instanceData, workflowData, paramsData, messages);
        };

        private It should_reactivate_user_or_round_robin_to_a_capitec_consultant = () =>
        {
            workflowAssignment.WasToldTo(x => x.ReactiveUserOrRoundRobinForOSKeyByProcess((IDomainMessageCollection)messages, "Branch Consultant D", applicationKey, 1158, instanceData.ID, "Capitec Applications",
                SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.CapitecConsultant));
        };

        private It should_set_case_owner_name_data_property = () =>
        {
            workflowData.CaseOwnerName.ShouldBeEqualIgnoringCase(assignedTo);
        };

        private It should_insert_commissionable_consultant = () =>
        {
            workflowAssignment.WasToldTo(x => x.InsertCommissionableConsultant((IDomainMessageCollection)messages,
                instanceData.ID,
                assignedTo,
                applicationKey,
                "Capitec Applications"));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}