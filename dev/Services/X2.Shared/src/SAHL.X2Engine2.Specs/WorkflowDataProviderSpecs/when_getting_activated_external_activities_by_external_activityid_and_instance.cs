using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.X2Engine2.Providers;

using SAHL.X2Engine2.ViewModels;
using SAHL.X2Engine2.ViewModels.SqlStatement;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;
using SAHL.Core.Testing.Providers;
using SAHL.Core.Testing.Fakes;

namespace SAHL.X2Engine2.Specs.WorkflowDataProviderSpecs
{
    public class when_getting_activated_external_activities_by_external_activityid_and_instance : WithFakes
    {
        static AutoMocker<WorkflowDataProvider> automocker = new NSubstituteAutoMocker<WorkflowDataProvider>();
        private static IReadOnlySqlRepository readOnlySqlRepository;
        private static ActivatedExternalActivitiesViewModel viewModel;
        private static IEnumerable<ActivatedExternalActivitiesViewModel> returnedModels;

        private Establish context = () =>
        {
            readOnlySqlRepository = MockRepositoryProvider.GetReadOnlyRepository();
            viewModel = new ActivatedExternalActivitiesViewModel(1, null, "externalactivity", 3);
            readOnlySqlRepository.WhenToldTo(x => x.Select<ActivatedExternalActivitiesViewModel>(Param.IsAny<ActivatedExternalActivitiesByExternalActivityIDAndInstanceId>())).
                Return(new List<ActivatedExternalActivitiesViewModel>(new ActivatedExternalActivitiesViewModel[] { viewModel }));
        };

        private Because of = () =>
        {
            returnedModels = automocker.ClassUnderTest.GetActivatedExternalActivitiesViewModelByExternalActivityIDandInstanceID(Param.IsAny<int>(), Param.IsAny<long?>());
        };

        private It should_return_the_correct_instance = () =>
        {
            returnedModels.ShouldContain<ActivatedExternalActivitiesViewModel>(x => x.InstanceId == 1 && x.ParentInstanceId == null && x.ExternalActivityTarget == 3);
        };
    }
}