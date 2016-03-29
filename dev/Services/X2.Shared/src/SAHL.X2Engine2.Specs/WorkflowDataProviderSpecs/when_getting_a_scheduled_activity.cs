using System;
using Machine.Fakes;
using Machine.Specifications;
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
    public class when_getting_a_scheduled_activity : WithFakes
    {
        static AutoMocker<WorkflowDataProvider> automocker = new NSubstituteAutoMocker<WorkflowDataProvider>();
        private static IReadOnlySqlRepository readOnlySqlRepository;
        private static ScheduledActivityDataModel scheduledActivity;
        private static ScheduledActivityDataModel results;
        private static long instanceId;
        private static int activityId;

        Establish context = () =>
        {
            instanceId = 1234567L;
            activityId = 999;
            readOnlySqlRepository = MockRepositoryProvider.GetReadOnlyRepository();
            scheduledActivity = new ScheduledActivityDataModel(1234567L, DateTime.Now, 1, 1, "WorkflowProviderName", 1);
            readOnlySqlRepository.WhenToldTo(x => x.SelectOne(Param.IsAny<ScheduledActivityForInstanceAndActivity>())).Return(scheduledActivity);
        };

        Because of = () =>
        {
            results = automocker.ClassUnderTest.GetScheduledActivity(instanceId, activityId);
        };

        It should_return_the_scheduled_activity_retrieved_from_the_database = () =>
        {
            results.ShouldBeTheSameAs(scheduledActivity);
        };
    }
}