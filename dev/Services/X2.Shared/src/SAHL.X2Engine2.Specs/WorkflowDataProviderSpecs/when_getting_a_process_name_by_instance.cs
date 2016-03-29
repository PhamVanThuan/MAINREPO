using System;
using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;
using SAHL.X2Engine2.Providers;

using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;
using SAHL.Core.Testing.Providers;
using SAHL.Core.Testing.Fakes;

namespace SAHL.X2Engine2.Specs.WorkflowDataProviderSpecs
{
    public class when_getting_a_process_name_by_instance : WithFakes
    {
        static AutoMocker<WorkflowDataProvider> automocker = new NSubstituteAutoMocker<WorkflowDataProvider>();
        private static IReadOnlySqlRepository readOnlySqlRepository;
        private static string processName = "";
        private static InstanceDataModel instanceDataModel;
        private static WorkFlowDataModel workflowDataModel;
        private static ProcessDataModel processDataModel;
        private static Dictionary<string, object> parameters = new Dictionary<string, object>();
        private static string coreUIStatementToExecute;

        Establish context = () =>
        {
            coreUIStatementToExecute = UIStatements.processdatamodel_selectbykey;
            readOnlySqlRepository = MockRepositoryProvider.GetReadOnlyRepository();
            instanceDataModel = new InstanceDataModel(10, 1, null, "name", "subject", "", null, "creator", DateTime.Now, null, null, null, "", null, 9, null, null);
            workflowDataModel = new WorkFlowDataModel(1, 1, null, "name", DateTime.Now, "storage", "key", 1, "subject", 1);
            processDataModel = new ProcessDataModel(1, null, "processName", "1.0.0.0", new byte[] { }, DateTime.Now, "10", ",", string.Empty, true);
            readOnlySqlRepository.WhenToldTo(x => x.SelectOne<WorkFlowDataModel>(Param.IsAny<string>(), Param.IsAny<object>())).Return(workflowDataModel);
            readOnlySqlRepository.WhenToldTo(x => x.SelectOne<ProcessDataModel>(Param.IsAny<string>(), Param.IsAny<object>())).Return(processDataModel);
        };

        Because of = () =>
        {
            processName = automocker.ClassUnderTest.GetProcessName(instanceDataModel);
        };

        It should_return_the_process_name = () =>
        {
            processName.ShouldEqual("processName");
        };

        It should_use_the_process_id_from_the_instances_workflow_when_running_the_core_uiStatement = () =>
        {
            readOnlySqlRepository.WasToldTo(x => x.SelectOne<ProcessDataModel>(coreUIStatementToExecute, Arg.Is<object>(anonymousObject => anonymousObject.CheckValue(parameters))));
        };
    }
}