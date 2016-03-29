using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Caching;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.X2Engine2.Providers;
using SAHL.X2Engine2.ViewModels.SqlStatement;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.WorkflowDataProviderSpecs
{
    public class when_getting_an_activity_by_activating_external_activity : WithFakes
    {
        static AutoMocker<WorkflowDataProvider> automocker = new NSubstituteAutoMocker<WorkflowDataProvider>();
        private static IReadOnlySqlRepository readOnlySqlRepository;
        private static ActivityDataModel results;
        private static ActivityDataModel activityModel;
        private static int externalActivityID;

        Establish context = () =>
        {
            externalActivityID = 999;
            activityModel = Helper.GetActivityDataModel();
            readOnlySqlRepository = MockRepositoryProvider.GetReadOnlyRepository();
            readOnlySqlRepository.WhenToldTo(x =>
                x.SelectOne<ActivityDataModel>(Arg.Is<ActivityByActivatingExternalActivityIdSqlStatement>(y => y.ExternalActivityId == externalActivityID)))
                .Return(activityModel);
            ;
        };

        Because of = () =>
        {
            results = automocker.ClassUnderTest.GetActivityByActivatingExternalActivity(externalActivityID, Param.IsAny<int>());
        };

        It should_add_it_to_the_cache = () =>
            {
                automocker.Get<ICache>().WasToldTo(x => x.AddItem<ActivityDataModel>(Param.IsAny<string>(), activityModel));
            };

        It should_return_the_activity_data_model_provided_by_the_database = () =>
        {
            results.ShouldBeTheSameAs(activityModel);
        };
    }
}