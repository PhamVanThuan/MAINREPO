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
    public class when_handling_for_an_existing_instance_request_that_is_a_split : WithFakes
    {
        private static AutoMocker<ExternalActivityCommandHandler> autoMocker = new NSubstituteAutoMocker<ExternalActivityCommandHandler>();
        private static ExternalActivityCommand command;
        static Activity activity;
        static string activityName = "Ext Activity";
        private static ActivityDataModel activityDataModel;
        static int activityID = 9;
        static InstanceDataModel instance;
        private static int ExternalActivityID = 1;
        private static int WorkFlowID = 12;
        private static long? ActivatingInstanceID = 99;
        private static DateTime ActivationTime = DateTime.Now;
        private static Dictionary<string, string> MapVariables = new Dictionary<string, string>();
        static IEnumerable<ActivatedExternalActivitiesViewModel> ActivatedExternalActivitiesViewModels;
        static ActivatedExternalActivitiesViewModel activatedExternalActivitiesViewModel;
        static ServiceRequestMetadata metadata;

        Establish context = () =>
        {
            autoMocker = new NSubstituteAutoMocker<ExternalActivityCommandHandler>();
            activatedExternalActivitiesViewModel = new ActivatedExternalActivitiesViewModel(ActivatingInstanceID, null, activityName, 1);
            ActivatedExternalActivitiesViewModels = new List<ActivatedExternalActivitiesViewModel>(new ActivatedExternalActivitiesViewModel[] { activatedExternalActivitiesViewModel });
            instance = new InstanceDataModel((long)ActivatingInstanceID, WorkFlowID, null, "name", "subject", "", 10, "creatir", DateTime.Now, DateTime.Now, null, null, null, null, 9, null, null);

            activity = new Activity(activityID, activityName, 1, "fromState", 2, "toState", WorkFlowID, true);
            activityDataModel = new ActivityDataModel(activityID, WorkFlowID, activityName, 3, 1, 2, true, 9, null, "", null, 2, 1, "", 1, Guid.NewGuid());

            command = new ExternalActivityCommand(ExternalActivityID, WorkFlowID, ActivatingInstanceID, ActivationTime, MapVariables);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetActivatedExternalActivitiesViewModelByExternalActivityIDandInstanceID(ExternalActivityID, ActivatingInstanceID)).Return(ActivatedExternalActivitiesViewModels);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetActivityByActivatingExternalActivity(ExternalActivityID, (int)instance.StateID)).Return(activityDataModel);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetActivityById(activityDataModel.ID)).Return(activity);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetInstanceDataModel(command.ActivatingInstanceID.Value)).Return(instance);
        };

        Because of = () =>
        {
            autoMocker.ClassUnderTest.HandleCommand(command, metadata);
        };

        It should_get_the_activated_external_activities_for_the_external_activity_and_instnace = () =>
        {
            autoMocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetActivatedExternalActivitiesViewModelByExternalActivityIDandInstanceID(ExternalActivityID, ActivatingInstanceID));
        };

        It should_get_the_activity_from_the_workflow_data_provider = () =>
        {
            autoMocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetActivityById(activityID));
        };

        It should_call_the_UserRequestStartActivityWithoutSplitCommandHandler = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Is<UserRequestStartActivityWithSplitCommand>(y => y.Activity.ActivityID == activityID), metadata));
        };

        It should_call_the_UserRequestCompleteActivityCommandHandler = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Is<UserRequestCompleteActivityCommand>(y => y.Activity.ActivityID == activityID), metadata));
        };
    }
}