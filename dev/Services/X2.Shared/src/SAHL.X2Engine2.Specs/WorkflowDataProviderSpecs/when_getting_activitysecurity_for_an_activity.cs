using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Caching;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;
using SAHL.X2Engine2.Providers;

using SAHL.X2Engine2.ViewModels.SqlStatement;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;
using SAHL.Core.Testing.Providers;
using SAHL.Core.Testing.Fakes;

namespace SAHL.X2Engine2.Specs.WorkflowDataProviderSpecs
{
    public class when_getting_activitysecurity_for_an_activity : WithFakes
    {
        static AutoMocker<WorkflowDataProvider> automocker = new NSubstituteAutoMocker<WorkflowDataProvider>();
        private static IReadOnlySqlRepository readOnlySqlRepository;
        static List<ActivitySecurityDataModel> items;
        private static ActivitySecurityDataModel activitySecurityDataModel;
        private static ActivitySecurityDataModel returnedModel;

        private Establish context = () =>
        {
            readOnlySqlRepository = MockRepositoryProvider.GetReadOnlyRepository();
            activitySecurityDataModel = new ActivitySecurityDataModel(10, 1, 1);
            items = new List<ActivitySecurityDataModel>(new ActivitySecurityDataModel[] { activitySecurityDataModel });
            readOnlySqlRepository.WhenToldTo(x => x.Select<ActivitySecurityDataModel>(Param.IsAny<ActivitySecurityByActivitySqlStatement>())).
                Return(items);
        };

        private Because of = () =>
        {
            returnedModel = automocker.ClassUnderTest.GetActivitySecurityForActivity(10).First();
        };

        It should_add_it_to_the_cache = () =>
            {
                automocker.Get<ICache>().WasToldTo(x => x.AddItem<IEnumerable<ActivitySecurityDataModel>>(Param.IsAny<string>(), items));
            };

        private It should_return_the_correct_instance = () =>
        {
            returnedModel.ID.ShouldEqual(10);
        };
    }
}