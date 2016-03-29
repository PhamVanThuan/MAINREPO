using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Caching;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;
using SAHL.X2Engine2.Providers;

using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;
using SAHL.Core.Testing.Providers;
using SAHL.Core.Testing.Fakes;

namespace SAHL.X2Engine2.Specs.WorkflowDataProviderSpecs
{
    public class when_getting_a_workflowdatamodel_by_id : WithFakes
    {
        static AutoMocker<WorkflowDataProvider> automocker = new NSubstituteAutoMocker<WorkflowDataProvider>();
        private static IReadOnlySqlRepository readOnlySqlRepository;
        private static WorkFlowDataModel workflowDataModel;
        private static WorkFlowDataModel returnModel;
        private static int workflowID;
        private static Dictionary<string, object> parameters = new Dictionary<string, object>();
        private static string coreUIStatementToExecute;

        Establish context = () =>
        {
            coreUIStatementToExecute = UIStatements.workflowdatamodel_selectbykey;
            readOnlySqlRepository = MockRepositoryProvider.GetReadOnlyRepository();
            workflowDataModel = Helper.GetWorkflowDataModel();
            workflowID = workflowDataModel.ID;
            parameters.Add("PrimaryKey", workflowDataModel.ID);
            readOnlySqlRepository.WhenToldTo(x => x.SelectOne<WorkFlowDataModel>(Param.IsAny<string>(), Param.IsAny<object>())).Return(workflowDataModel);
        };

        Because of = () =>
        {
            returnModel = automocker.ClassUnderTest.GetWorkflowById(workflowID);
        };

        It should_return_the_correct_instance = () =>
        {
            returnModel.ShouldBeTheSameAs(workflowDataModel);
        };

        It should_add_it_to_the_cache = () =>
            {
                automocker.Get<ICache>().WasToldTo(x => x.AddItem<WorkFlowDataModel>(Param.IsAny<string>(), workflowDataModel));
            };

        It should_use_the_workflow_id_provided_when_running_the_core_uiStatement = () =>
        {
            readOnlySqlRepository.WasToldTo(x => x.SelectOne<WorkFlowDataModel>(coreUIStatementToExecute, Arg.Is<object>(anonymousObject => anonymousObject.CheckValue(parameters))));
        };
    }
}