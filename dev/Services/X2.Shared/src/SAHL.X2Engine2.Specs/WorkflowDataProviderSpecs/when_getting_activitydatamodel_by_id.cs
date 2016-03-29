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
    public class when_getting_activitydatamodel_by_id : WithFakes
    {
        static AutoMocker<WorkflowDataProvider> automocker = new NSubstituteAutoMocker<WorkflowDataProvider>();
        private static IReadOnlySqlRepository readOnlySqlRepository;
        private static ActivityDataModel activityDataModel;
        private static ActivityDataModel returnModel;
        private static int activityID;
        private static Dictionary<string, object> parameters = new Dictionary<string, object>();
        private static string coreUIStatementToExecute;

        Establish context = () =>
        {
            coreUIStatementToExecute = UIStatements.activitydatamodel_selectbykey;
            readOnlySqlRepository = MockRepositoryProvider.GetReadOnlyRepository();
            activityDataModel = Helper.GetActivityDataModel();
            activityID = activityDataModel.ID;
            readOnlySqlRepository.WhenToldTo(x => x.SelectOne<ActivityDataModel>(Param.IsAny<string>(), Param.IsAny<object>())).Return(activityDataModel);
            parameters.Add("PrimaryKey", activityID);
        };

        Because of = () =>
        {
            returnModel = automocker.ClassUnderTest.GetActivity(activityID);
        };

        It should_return_the_correct_activity_data_model = () =>
        {
            returnModel.ShouldBeTheSameAs(activityDataModel);
        };

        It should_add_it_to_the_cache = () =>
            {
                automocker.Get<ICache>().WasToldTo(x => x.AddItem<ActivityDataModel>(Param.IsAny<string>(), activityDataModel));
            };

        It should_use_the_activity_id_provided_when_running_the_core_UIStatement = () =>
        {
            readOnlySqlRepository.WasToldTo(x => x.SelectOne<ActivityDataModel>(coreUIStatementToExecute, Arg.Is<object>(anon => anon.CheckValue(parameters))));
        };
    }
}