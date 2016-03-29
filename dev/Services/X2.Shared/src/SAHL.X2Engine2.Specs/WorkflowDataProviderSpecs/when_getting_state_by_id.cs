using System;
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
    public class when_getting_state_by_id : WithFakes
    {
        static AutoMocker<WorkflowDataProvider> automocker = new NSubstituteAutoMocker<WorkflowDataProvider>();
        private static IReadOnlySqlRepository readOnlySqlRepository;
        private static StateDataModel stateDataModel;
        private static StateDataModel returnedModel;
        private static Dictionary<string, object> parameters = new Dictionary<string, object>();
        private static string coreStatementToExecute;

        Establish context = () =>
        {
            coreStatementToExecute = UIStatements.statedatamodel_selectbykey;
            readOnlySqlRepository = MockRepositoryProvider.GetReadOnlyRepository();
            stateDataModel = new StateDataModel(10, 1, "state", 1, false, null, null, null, Guid.NewGuid());
            parameters.Add("PrimaryKey", stateDataModel.ID);
            readOnlySqlRepository.WhenToldTo(x => x.SelectOne<StateDataModel>(Param.IsAny<string>(), Param.IsAny<object>())).Return(stateDataModel);
        };

        Because of = () =>
        {
            returnedModel = automocker.ClassUnderTest.GetStateById(10);
        };

        It should_return_the_correct_instance = () =>
        {
            returnedModel.ShouldBeTheSameAs(stateDataModel);
        };

        It should_add_it_to_the_cache = () =>
            {
                automocker.Get<ICache>().WasToldTo(x => x.AddItem(Param.IsAny<string>(), stateDataModel));
            };

        It should_use_the_state_id_provided_when_running_core_uiStatement = () =>
        {
            readOnlySqlRepository.WasToldTo(x => x.SelectOne<StateDataModel>(coreStatementToExecute, Arg.Is<object>(anonymousObject => anonymousObject.CheckValue(parameters))));
        };
    }
}