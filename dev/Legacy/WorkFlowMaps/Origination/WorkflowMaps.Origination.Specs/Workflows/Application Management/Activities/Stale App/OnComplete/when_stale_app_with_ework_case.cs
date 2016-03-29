using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Stale_App.OnComplete
{
    [Subject("Activity => Stale_App => OnComplete")]
    internal class when_stale_app_has_ework_case : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string message;
        private static IFL furtherLendingClient;
        private static IApplicationManagement applicationManagementClient;
        private static IWorkflowAssignment workflowAssignmentClient;
        private static ICommon commonClient;
        private static string expectedEworkId;

        private Establish context = () =>
        {
            result = false;
            message = String.Empty;
            workflowData.IsFL = false;
            expectedEworkId = "123456789123456";
            workflowData.EWorkFolderID = expectedEworkId;

            //Make fakes
            furtherLendingClient = An<IFL>();
            applicationManagementClient = An<IApplicationManagement>();
            workflowAssignmentClient = An<IWorkflowAssignment>();
            commonClient = An<ICommon>();

            domainServiceLoader.RegisterMockForType<IFL>(furtherLendingClient);
            domainServiceLoader.RegisterMockForType<IApplicationManagement>(applicationManagementClient);
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(workflowAssignmentClient);
            domainServiceLoader.RegisterMockForType<ICommon>(commonClient);

            applicationManagementClient.WhenToldTo(x => x.SendEmailToConsultantForQuery((IDomainMessageCollection)messages,
                                  workflowData.ApplicationKey, instanceData.ID, 4));
            commonClient.WhenToldTo(x => x.PerformEWorkAction((IDomainMessageCollection)messages, expectedEworkId, SAHL.Common.Constants.EworkActionNames.X2NTUAdvise, workflowData.ApplicationKey,
                                                                paramsData.ADUserName, paramsData.StateName)).Return(true);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Stale_App(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_perform_x2_ntu_advise_ework_action = () =>
        {
            commonClient.WasToldTo(x => x.PerformEWorkAction((IDomainMessageCollection)messages, expectedEworkId, SAHL.Common.Constants.EworkActionNames.X2NTUAdvise, workflowData.ApplicationKey,
                                                                paramsData.ADUserName, paramsData.StateName));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}