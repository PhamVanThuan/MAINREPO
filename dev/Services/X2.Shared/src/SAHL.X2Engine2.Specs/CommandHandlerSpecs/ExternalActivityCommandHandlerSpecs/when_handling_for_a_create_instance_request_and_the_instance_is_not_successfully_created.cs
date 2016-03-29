using System;
using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2.Factories;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Providers;
using SAHL.X2Engine2.ViewModels;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.ExternalActivityCommandHandlerSpecs
{
    public class when_handling_for_a_create_instance_request_and_the_instance_is_not_successfully_created : WithFakes
    {
        private static AutoMocker<ExternalActivityCommandHandler> autoMocker = new NSubstituteAutoMocker<ExternalActivityCommandHandler>();
        private static ExternalActivityCommand command;
        private static Activity activity;
        private static ActivityDataModel activityDataModel;
        private static WorkFlowDataModel workFlowDataModel;
        private static ProcessDataModel processDataModel;
        private static InstanceDataModel newlyCreatedInstance;
        private static string activityName;
        private static int workFlowID;
        private static Dictionary<string, string> MapVariables = new Dictionary<string, string>();
        private static IEnumerable<ActivatedExternalActivitiesViewModel> activatedExternalActivitiesViewModels;
        private static ActivatedExternalActivitiesViewModel activatedExternalActivitiesViewModel;
        private static ISystemMessageCollection messages;
        private static System.Exception exception;
        private static int externalActivityID;
        static ServiceRequestMetadata metadata;

        Establish context = () =>
        {
            workFlowID = 1;
            activityName = "Create External Activity";
            externalActivityID = 1;
            messages = An<ISystemMessageCollection>();
            autoMocker = new NSubstituteAutoMocker<ExternalActivityCommandHandler>();
            activatedExternalActivitiesViewModel = new ActivatedExternalActivitiesViewModel(-1, null, activityName, 3);
            activatedExternalActivitiesViewModels = new List<ActivatedExternalActivitiesViewModel>(new ActivatedExternalActivitiesViewModel[] { activatedExternalActivitiesViewModel });
            autoMocker.Get<IMessageCollectionFactory>().WhenToldTo(x => x.CreateEmptyCollection()).Return(messages);

            newlyCreatedInstance = new InstanceDataModel(9, 1, null, "name", "subject", "workflowProvider", 1, "CreatorADUserName", DateTime.Now, null, null, null, null, null, null, null, null);
            activity = new Activity(1, activityName, null, "", 3, "toState", workFlowID, false);
            activityDataModel = new ActivityDataModel(1, workFlowID, activityName, 3, null, 2, false, 9, null, "", null, 3, 1, "", 1, Guid.NewGuid());
            workFlowDataModel = new WorkFlowDataModel(0, null, "name", DateTime.Now, "storage table", "storage key", 0, "default subject", null);
            processDataModel = new ProcessDataModel(null, "name", "version", new byte[] { }, DateTime.Now, "map version", "config file", string.Empty, true);

            command = new ExternalActivityCommand(externalActivityID, workFlowID, newlyCreatedInstance.ID, DateTime.Now, MapVariables);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetActivatedExternalActivitiesViewModelByExternalActivityIDandInstanceID(externalActivityID, command.ActivatingInstanceID)).Return(activatedExternalActivitiesViewModels);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetActivityByActivatingExternalActivity(externalActivityID, Param.IsAny<int>())).Return(activityDataModel);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetActivityById(activityDataModel.ID)).Return(activity);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetWorkflowById(workFlowID)).Return(workFlowDataModel);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetProcessById(workFlowDataModel.ProcessID)).Return(processDataModel);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetInstanceDataModel(activatedExternalActivitiesViewModel.InstanceId.Value)).Return(newlyCreatedInstance);

            autoMocker.Get<IX2ServiceCommandRouter>().WhenToldTo<IX2ServiceCommandRouter>(
                x => x.HandleCommand(Arg.Is<UserRequestCreateInstanceCommand>(c => c.WorkflowName == workFlowDataModel.Name && c.ProcessName == processDataModel.Name), metadata)).
                Callback<UserRequestCreateInstanceCommand>((c) =>
                {
                    c.NewlyCreatedInstanceId = 0;
                });
        };

        Because of = () =>
        {
            exception = Catch.Exception(() =>
            {
                messages = autoMocker.ClassUnderTest.HandleCommand(command, metadata);
            });
        };

        It should_throw_an_exception = () =>
        {
            exception.Message.ShouldEqual(string.Format("Unable to create instance for external activity:{0}", command.ExternalActivityID));
        };

        It should_not_complete_the_create_of_the_instance = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasNotToldTo<IX2ServiceCommandRouter>(
                x => x.HandleCommand(Arg.Is<UserRequestCompleteCreateCommand>(Arg.Any<UserRequestCompleteCreateCommand>()), metadata));
        };
    }
}