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
    public class when_getting_an_instance_by_id : WithFakes
    {
        static AutoMocker<WorkflowDataProvider> automocker = new NSubstituteAutoMocker<WorkflowDataProvider>();
        private static IReadOnlySqlRepository readOnlySqlRepository;
        private static InstanceDataModel instanceDataModel;
        private static InstanceDataModel returnedModel;
        private static long instanceId;
        private static string coreUIStatementToExecute;
        private static Dictionary<string, object> parameters = new Dictionary<string, object>();

        Establish context = () =>
        {
            coreUIStatementToExecute = UIStatements.instancedatamodel_selectbykey;
            instanceId = 1234657L;
            parameters.Add("PrimaryKey", instanceId);
            readOnlySqlRepository = MockRepositoryProvider.GetReadOnlyRepository();
            instanceDataModel = Helper.GetInstanceDataModel(instanceId);
            readOnlySqlRepository.WhenToldTo(x => x.SelectOne<InstanceDataModel>(Param.IsAny<string>(), Param.IsAny<object>())).Return(instanceDataModel);
        };

        Because of = () =>
        {
            returnedModel = automocker.ClassUnderTest.GetInstanceDataModel(instanceId);
        };

        It should_return_the_correct_instance = () =>
        {
            returnedModel.ShouldBeTheSameAs(instanceDataModel);
        };

        It should_use_the_instance_id_provided_when_running_the_core_uiStatement = () =>
        {
            readOnlySqlRepository.WasToldTo(x => x.SelectOne<InstanceDataModel>(coreUIStatementToExecute, Arg.Is<object>(anon => anon.CheckValue(parameters))));
        };
    }
}