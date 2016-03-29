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
    internal class when_qa_query_and_is_fl_and_insufficient_income : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string message;
        private static IApplicationManagement appMan;
        private static IWorkflowAssignment assignment;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
            workflowData.IsFL = true;
            ((InstanceDataStub)instanceData).ID = 1;
            workflowData.ApplicationKey = 2;

            appMan = An<IApplicationManagement>();
            domainServiceLoader.RegisterMockForType<IApplicationManagement>(appMan);

            assignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignment);

            var common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);
            common.WhenToldTo(x => x.CheckApplicationMinimumIncomeRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>()))
                .Return(false);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_QA_Query(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_not_send_email_to_consultant_for_query = () =>
        {
            appMan.WasNotToldTo(x => x.SendEmailToConsultantForQuery((IDomainMessageCollection)messages, workflowData.ApplicationKey, instanceData.ID, (int)SAHL.Common.Globals.ReasonTypeGroups.Query));
        };

        private It should_not_deactive_users_for_instance = () =>
        {
            assignment.WasNotToldTo(x => x.DeActiveUsersForInstanceAndProcess(Param.IsAny<IDomainMessageCollection>(),
                Param.IsAny<long>(),
                Param.IsAny<int>(),
                Param.IsAny<List<string>>(),
                Param.IsAny<SAHL.Common.Globals.Process>()));
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}