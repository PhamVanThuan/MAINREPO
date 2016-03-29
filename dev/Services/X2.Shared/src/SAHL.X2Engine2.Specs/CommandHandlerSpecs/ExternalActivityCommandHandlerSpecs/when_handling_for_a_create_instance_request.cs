using System;
using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Providers;
using SAHL.X2Engine2.ViewModels;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.ExternalActivityCommandHandlerSpecs
{
    public class when_handling_for_a_create_instance_request : WithFakes
    {
        private static AutoMocker<ExternalActivityCommandHandler> autoMocker = new NSubstituteAutoMocker<ExternalActivityCommandHandler>();
        private static ExternalActivityCommand command;
        private static Activity activity;
        private static ActivityDataModel activityDataModel;
        private static WorkFlowDataModel workFlowDataModel;
        private static ProcessDataModel processDataModel;
        private static int ExternalActivityID = 1;
        private static string ActivityName = "Create External Activity";
        private static int WorkFlowID = 12;
        private static long? ActivatingInstanceID = null;
        private static DateTime ActivationTime = DateTime.Now;
        private static Dictionary<string, string> MapVariables = new Dictionary<string, string>();
        private static IEnumerable<ActivatedExternalActivitiesViewModel> ActivatedExternalActivitiesViewModels;
        private static ActivatedExternalActivitiesViewModel activatedExternalActivitiesViewModel;
        static ServiceRequestMetadata metadata;

        Establish context = () =>
        {
            autoMocker = new NSubstituteAutoMocker<ExternalActivityCommandHandler>();
            activatedExternalActivitiesViewModel = new ActivatedExternalActivitiesViewModel(ActivatingInstanceID, null, ActivityName, 3);
            ActivatedExternalActivitiesViewModels = new List<ActivatedExternalActivitiesViewModel>(new ActivatedExternalActivitiesViewModel[] { activatedExternalActivitiesViewModel });

            activity = new Activity(1, ActivityName, null, "", 3, "toState", WorkFlowID, false);
            activityDataModel = new ActivityDataModel(1, WorkFlowID, ActivityName, 3, null, 2, false, 9, null, "", null, 3, 1, "", 1, Guid.NewGuid());
            workFlowDataModel = new WorkFlowDataModel(0, null, "name", DateTime.Now, "storage table", "storage key", 0, "default subject", null);
            processDataModel = new ProcessDataModel(null, "name", "version", new byte[] { }, DateTime.Now, "map version", "config file", string.Empty, true);

            command = new ExternalActivityCommand(ExternalActivityID, WorkFlowID, ActivatingInstanceID, ActivationTime, MapVariables);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetActivatedExternalActivitiesViewModelByExternalActivityIDandInstanceID(ExternalActivityID, command.ActivatingInstanceID)).Return(ActivatedExternalActivitiesViewModels);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetActivityByActivatingExternalActivity(ExternalActivityID, -1)).Return(activityDataModel);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetActivityById(activityDataModel.ID)).Return(activity);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetWorkflowById(WorkFlowID)).Return(workFlowDataModel);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetProcessById(workFlowDataModel.ProcessID)).Return(processDataModel);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetInstanceDataModel(-1)).Return(null as InstanceDataModel);

            autoMocker.Get<IX2ServiceCommandRouter>().WhenToldTo<IX2ServiceCommandRouter>(
                x => x.HandleCommand(Arg.Is<UserRequestCreateInstanceCommand>(c => c.WorkflowName == workFlowDataModel.Name && c.ProcessName == processDataModel.Name), metadata))
                .Callback<UserRequestCreateInstanceCommand>((c) =>
                {
                    c.NewlyCreatedInstanceId = 999;
                });
        };

        Because of = () =>
        {
            autoMocker.ClassUnderTest.HandleCommand(command, metadata);
        };

        It should_get_the_activated_external_activities_for_the_external_activity_and_instance = () =>
        {
            autoMocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetActivatedExternalActivitiesViewModelByExternalActivityIDandInstanceID(command.ExternalActivityID, activatedExternalActivitiesViewModel.InstanceId));
        };

        It should_get_the_workflow_from_the_workflow_data_provider = () =>
        {
            autoMocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetWorkflowById(WorkFlowID));
        };

        It should_get_the_process_from_the_workflow_data_provider = () =>
        {
            autoMocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetProcessById(workFlowDataModel.ProcessID));
        };

        It should_call_the_UserRequestCreateInstanceCommandHandler = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Any<UserRequestCreateInstanceCommand>(), metadata));
        };

        It should_call_the_UserRequestCompleteCreateCommandHandler = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Is<UserRequestCompleteCreateCommand>(y => y.InstanceId == 999), metadata));
        };
    }
}