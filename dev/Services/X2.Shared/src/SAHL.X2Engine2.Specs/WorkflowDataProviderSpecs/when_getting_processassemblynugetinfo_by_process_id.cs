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
    public class when_getting_processassemblynugetinfo_by_process_id : WithFakes
    {
        private static AutoMocker<WorkflowDataProvider> automocker = new NSubstituteAutoMocker<WorkflowDataProvider>();
        private static IReadOnlySqlRepository readOnlySqlRepository;
        private static ProcessAssemblyNugetInfoDataModel dataModel;
        private static ProcessAssemblyNugetInfoDataModel returnModel;
        private static List<ProcessAssemblyNugetInfoDataModel> models = new List<ProcessAssemblyNugetInfoDataModel>();

        Establish context = () =>
        {
            readOnlySqlRepository = MockRepositoryProvider.GetReadOnlyRepository();
            dataModel = new ProcessAssemblyNugetInfoDataModel(1, 1, "Package", "Version");
            models.Add(dataModel);
            readOnlySqlRepository.WhenToldTo(x => x.Select<ProcessAssemblyNugetInfoDataModel>(Param.IsAny<ProcessAssemblyNugetInfoByProcessIdSqlStatement>())).Return(models);
        };

        Because of = () =>
        {
            returnModel = automocker.ClassUnderTest.GetProcessAssemblyNuGetInfoByProcessId(1).FirstOrDefault();
        };

        It should_add_them_to_the_cache = () =>
        {
            automocker.Get<ICache>().WasToldTo(x => x.AddItem<IEnumerable<ProcessAssemblyNugetInfoDataModel>>(Param.IsAny<string>(), models));
        };

        It should_return_system_activities_coming_from_that_state = () =>
        {
            returnModel.ID.ShouldEqual(1);
        };
    }
}