using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.ApplicationManagement.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.QA_Query.OnComplete
{
    [Subject("Activity => QA_Query => OnComplete")]
    internal class when_qa_query_and_is_not_fl : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string message;
        private static IApplicationManagement appMan;
        private static IWorkflowAssignment assignment;
        private static List<string> dys;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
            workflowData.IsFL = false;
            dys = new List<string>() { "QA Administrator D" };
            ((InstanceDataStub)instanceData).ID = 1;
            workflowData.AppCapIID = 2;
            workflowData.ApplicationKey = 3;

            appMan = An<IApplicationManagement>();
            domainServiceLoader.RegisterMockForType<IApplicationManagement>(appMan);

            assignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignment);

            var common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);
            common.WhenToldTo(x => x.CheckApplicationMinimumIncomeRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>())).Return(true);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_QA_Query(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_send_email_to_consultant_for_query = () =>
        {
            appMan.WasToldTo(x => x.SendEmailToConsultantForQuery((IDomainMessageCollection)messages, workflowData.ApplicationKey, instanceData.ID, (int)SAHL.Common.Globals.ReasonTypeGroups.Query));
        };

        private It should_deactive_users_for_instance = () =>
        {
            assignment.WasToldTo(x => x.DeActiveUsersForInstanceAndProcess((IDomainMessageCollection)messages,
                instanceData.ID,
                workflowData.ApplicationKey,
                dys,
                SAHL.Common.Globals.Process.Origination));
        };

        private It should_reactive_branch_users_for_origination = () =>
        {
            assignment.WasToldTo(x => x.ReActivateBranchUsersForOrigination((IDomainMessageCollection)messages,
                instanceData.ID,
                workflowData.AppCapIID,
                workflowData.ApplicationKey,
                "Request at QA",
                SAHL.Common.Globals.Process.Origination));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}