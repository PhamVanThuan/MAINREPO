using System;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.Core.X2;
using SAHL.Core.X2.Providers;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Providers;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.RebuildInstanceCommandHandlerSpecs
{
    public class when_rebuilding_an_x2_instance : WithFakes
    {
        private static RebuildInstanceCommand command;
        private static AutoMocker<RebuildInstanceCommandHandler> automocker;
        private static long instanceId = 10;
        static InstanceDataModel instanceDataModel;
        static IX2Process process;
        static IX2Map map;
        static IX2ContextualDataProvider contextualData;
        static string workflowName = "workflow";
        static ServiceRequestMetadata metadata;

        Establish context = () =>
            {
                automocker = new NSubstituteAutoMocker<RebuildInstanceCommandHandler>();
                command = new RebuildInstanceCommand(instanceId);

                instanceDataModel = new InstanceDataModel(instanceId, 1, 1, "name", "subject", "", 1, "", DateTime.Now, null, null, null, "", null, null, null, null);
                automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetInstanceDataModel(instanceId)).Return(instanceDataModel);
                automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetWorkflowName(instanceDataModel)).Return(workflowName);

                contextualData = An<IX2ContextualDataProvider>();
                map = An<IX2Map>();
                map.WhenToldTo(x => x.GetContextualData(Arg.Any<long>())).Return(contextualData);
                process = An<IX2Process>();
                process.WhenToldTo(x => x.GetWorkflowMap(workflowName)).Return(map);
                automocker.Get<IX2ProcessProvider>().WhenToldTo(x => x.GetProcessForInstance(instanceId)).Return(process);
            };

        Because of = () =>
            {
                automocker.ClassUnderTest.HandleCommand(command, metadata);
            };

        It should_get_a_process = () =>
            {
                automocker.Get<IX2ProcessProvider>().WasToldTo(x => x.GetProcessForInstance(instanceId));
            };

        It should_get_an_instance_datamodel = () =>
            {
                automocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetInstanceDataModel(instanceId));
            };

        It should_get_a_workflowName = () =>
            {
                automocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetWorkflowName(instanceDataModel));
            };

        It should_get_a_map = () =>
            {
                process.WasToldTo(x => x.GetWorkflowMap(workflowName));
            };

        It should_get_contextual_data = () =>
            {
                map.WasToldTo(x => x.GetContextualData(Arg.Any<long>()));
            };

        It should_load_the_contextual_data = () =>
            {
                contextualData.WasToldTo(x => x.LoadData(instanceId));
            };

        It should_call_refreshworklistcommand = () =>
            {
                automocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Any<RefreshWorklistCommand>(), metadata));
            };

        It should_call_refresh_instance_activity_security_command = () =>
            {
                automocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Any<RefreshInstanceActivitySecurityCommand>(), metadata));
            };
    }
}