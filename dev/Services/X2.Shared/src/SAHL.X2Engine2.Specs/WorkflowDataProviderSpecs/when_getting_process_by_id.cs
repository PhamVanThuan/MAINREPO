using System;
using Machine.Fakes;
using Machine.Specifications;
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
    public class when_getting_process_by_id : WithFakes
    {
        static AutoMocker<WorkflowDataProvider> automocker = new NSubstituteAutoMocker<WorkflowDataProvider>();
        private static IReadOnlySqlRepository readOnlySqlRepository;
        private static ProcessDataModel processDataModel;
        private static ProcessDataModel returnModel;

        private Establish context = () =>
        {
            readOnlySqlRepository = MockRepositoryProvider.GetReadOnlyRepository();
            processDataModel = new ProcessDataModel(1, 1, "processName", "", new byte[] { }, DateTime.Now, "", ",", string.Empty, true);
            readOnlySqlRepository.WhenToldTo(x => x.SelectOne<ProcessDataModel>(Param.IsAny<string>(), Param.IsAny<object>())).Return(processDataModel);
        };

        private Because of = () =>
        {
            returnModel = automocker.ClassUnderTest.GetProcessById(1);
        };

        It should_add_it_to_the_cache = () =>
        {
            automocker.Get<ICache>().WasToldTo(x => x.AddItem<ProcessDataModel>(Param.IsAny<string>(), returnModel));
        };

        private It should_return_the_correct_instance = () =>
        {
            returnModel.ID.ShouldEqual(1);
        };
    }
}