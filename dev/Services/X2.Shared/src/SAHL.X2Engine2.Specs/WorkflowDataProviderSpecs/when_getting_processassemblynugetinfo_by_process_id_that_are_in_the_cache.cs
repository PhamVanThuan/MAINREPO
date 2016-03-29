using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Caching;
using SAHL.Core.Data.Models.X2;
using SAHL.X2Engine2.Providers;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.WorkflowDataProviderSpecs
{
    public class when_getting_processassemblynugetinfo_by_process_id_that_are_in_the_cache : WithFakes
    {
        static AutoMocker<WorkflowDataProvider> automocker = new NSubstituteAutoMocker<WorkflowDataProvider>();
        private static ProcessAssemblyNugetInfoDataModel dataModel;
        private static ProcessAssemblyNugetInfoDataModel returnModel;
        private static List<ProcessAssemblyNugetInfoDataModel> models = new List<ProcessAssemblyNugetInfoDataModel>();

        Establish context = () =>
        {
            dataModel = new ProcessAssemblyNugetInfoDataModel(1, 1, "Package", "Version");
            models.Add(dataModel);

            automocker.Get<ICacheKeyGenerator>().WhenToldTo(x => x.GetKey<IKeyedCacheKey, IEnumerable<ProcessAssemblyNugetInfoDataModel>>(Param.IsAny<IKeyedCacheKey>())).Return("Key");
            automocker.Get<ICache>().WhenToldTo(x => x.Contains("Key")).Return(true);
            automocker.Get<ICache>().WhenToldTo(x => x.GetItem<IEnumerable<ProcessAssemblyNugetInfoDataModel>>("Key")).Return(models);
        };

        Because of = () =>
        {
            returnModel = automocker.ClassUnderTest.GetProcessAssemblyNuGetInfoByProcessId(1).FirstOrDefault();
        };

        It should_get_them_from_the_cache = () =>
        {
            automocker.Get<ICache>().WasToldTo(x => x.GetItem<IEnumerable<ProcessAssemblyNugetInfoDataModel>>("Key"));
        };

        It should_return_system_activities_coming_from_that_state = () =>
        {
            returnModel.ID.ShouldEqual(1);
        };
    }
}