using System;
using System.Collections.Generic;
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
    public class when_getting_scheduled_activities : WithFakes
    {
        static AutoMocker<WorkflowDataProvider> automocker = new NSubstituteAutoMocker<WorkflowDataProvider>();
        private static IReadOnlySqlRepository readOnlySqlRepository;
        private static ScheduledActivityDataModel scheduledActivityOne;
        private static ScheduledActivityDataModel scheduledActivityTwo;
        private static IEnumerable<ScheduledActivityDataModel> scheduledActivitiesExpected;
        private static IEnumerable<ScheduledActivityDataModel> results;

        Establish context = () =>
        {
            scheduledActivityOne = new ScheduledActivityDataModel(1234567L, DateTime.Now, 1, 1, "WorkflowProviderName", 1);
            scheduledActivityTwo = new ScheduledActivityDataModel(123456789L, DateTime.Now, 1, 1, "WorkflowProviderName", 1);
            scheduledActivitiesExpected = new[] { scheduledActivityOne, scheduledActivityTwo };
            readOnlySqlRepository = MockRepositoryProvider.GetReadOnlyRepository();
            readOnlySqlRepository.WhenToldTo(x => x.Select(Param.IsAny<GetAllScheduledTimerActivitiesSqlStatement>())).Return(scheduledActivitiesExpected);
        };

        Because of = () =>
        {
            results = automocker.ClassUnderTest.GetAllScheduledTimerActivities();
        };

        It should_return_a_set_of_scheduled_activities_provided_from_the_database = () =>
        {
            results.ShouldBeTheSameAs(scheduledActivitiesExpected);
        };
    }
}