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
    public class when_getting_an_activity_by_activity_id : WithFakes
    {
        static AutoMocker<WorkflowDataProvider> automocker = new NSubstituteAutoMocker<WorkflowDataProvider>();
        private static IReadOnlySqlRepository readOnlySqlRepository;
        private static ActivityDataModel results;
        private static ActivityDataModel activityModel;
        private static int activityID;
        private static Dictionary<string, object> parameters = new Dictionary<string, object>();

        Establish context = () =>
        {
            activityID = 999;
            parameters.Add("PrimaryKey", activityID);
            activityModel = Helper.GetActivityDataModel();
            readOnlySqlRepository = MockRepositoryProvider.GetReadOnlyRepository();
            readOnlySqlRepository.WhenToldTo(x => x.SelectOne<ActivityDataModel>(Param.IsAny<string>(), Param.IsAny<object>())).Return(activityModel);
        };

        Because of = () =>
        {
            results = automocker.ClassUnderTest.GetActivity(activityID);
        };

        It should_return_the_activity_data_model_provided_by_the_database = () =>
        {
            results.ShouldBeTheSameAs(activityModel);
        };

        It should_add_it_to_the_cache = () =>
            {
                automocker.Get<ICache>().WasToldTo(x => x.AddItem<ActivityDataModel>(Param.IsAny<string>(), activityModel));
            };

        It should_use_the_activity_id_provided = () =>
        {
            readOnlySqlRepository.WasToldTo(x => x.SelectOne<ActivityDataModel>(Param.IsAny<string>(), Arg.Is<object>(anonymousObject => anonymousObject.CheckValue(parameters))));
        };
    }
}