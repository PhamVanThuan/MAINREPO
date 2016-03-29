using System;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Communication;
using SAHL.X2Engine2.Node.Providers;
using SAHL.X2Engine2.ViewModels;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.NodeHandlerSpecs.ExternalHandlerSpecs
{
    public class when_queueing_up_external_activities_where_the_activity_raises_an_external_activity : WithFakes
    {
        private static AutoMocker<QueueUpExternalActivitiesCommandHandler> autoMocker;
        private static IReadWriteSqlRepository readWriteSqlRepository;
        private static QueueUpExternalActivitiesCommand command;
        private static InstanceDataModel instance;
        private static Activity activity;
        private static X2RouteEndpoint engineRoute;
        static ServiceRequestMetadata metadata;
        private static X2Workflow workflow;

        private Establish context = () =>
        {
            workflow = new X2Workflow("process","workflow");
            engineRoute = new X2RouteEndpoint("exchange", "queue");
            readWriteSqlRepository = An<IReadWriteSqlRepository>();
            instance = new InstanceDataModel(9, 1, null, "name", "subject", "workflowProvider", 1, "CreatorADUserName", DateTime.Now, null, null, null, null, null, null, null, null);
            activity = new Activity(1, "DoSomething", 1, "state", 2, "nextState", 999, false, raiseExternalActivityId: 5);
            activity.WorkflowId = 1;
            autoMocker = new NSubstituteAutoMocker<QueueUpExternalActivitiesCommandHandler>();
            autoMocker.Get<IX2RequestInterrogator>().WhenToldTo(x => x.GetRequestWorkflow(Param.IsAny<IX2Request>())).Return(workflow);
            autoMocker.Get<IX2QueueNameBuilder>().WhenToldTo(x => x.GetSystemQueue(workflow)).Return(engineRoute);
            command = new QueueUpExternalActivitiesCommand(instance, activity);
        };

        private Because of = () =>
        {
            autoMocker.ClassUnderTest.HandleCommand(command, metadata);
        };

        private It should_publish_the_external_activity_request = () =>
        {
            autoMocker.Get<IX2RequestPublisher>().WasToldTo(x => x.Publish<X2ExternalActivityRequest>(engineRoute
                , Param<X2ExternalActivityRequest>.Matches(request => request.InstanceId == command.Instance.ID
                    && request.ExternalActivityId == command.Activity.RaiseExternalActivityId
                    && request.ActivatingInstanceId == command.Instance.ID
                    && request.WorkflowId == command.Instance.WorkFlowID)));
        };
    }
}