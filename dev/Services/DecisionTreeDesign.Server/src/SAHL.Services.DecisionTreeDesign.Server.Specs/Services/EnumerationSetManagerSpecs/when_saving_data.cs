using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Services.DecisionTreeDesign.Managers.EnumerationSet;
using System;

namespace SAHL.Services.DecisionTreeDesign.Server.Specs.Services.EnumerationSetManagerSpecs
{
    [Subject("SAHL.Services.DecisionTree.Services.EnumerationSetManager.SaveEnumerationSet")]
    public class when_saving_data : WithFakes
    {
        private static IEnumerationSetManager enumManager;
        private static IEnumerationSetDataManager enumDataService;
        private static IUnitOfWorkFactory unitOfWorkFactory;

        private static Guid id;
        private static int version;
        private static string data;

        private Establish context = () =>
        {
            id = Guid.Empty;
            version = 0;

            enumDataService = An<IEnumerationSetDataManager>();
            unitOfWorkFactory = An<IUnitOfWorkFactory>();
            enumDataService.WhenToldTo(x => x.IsEnumerationSetVersionPublished(version)).Return(true);
            enumManager = new EnumerationSetManager(enumDataService, unitOfWorkFactory);
        };

        private Because of = () =>
        {
            enumManager.SaveEnumerationSet(id, version, data);
        };

        private It should_determine_if_current_version_is_published = () =>
        {
            enumDataService.WasToldTo(x => x.IsEnumerationSetVersionPublished(version));
        };

        private It should_save_enumeration_set = () =>
        {
            enumDataService.WasToldTo(x => x.InsertEnumerationSet(Param.IsAny<Guid>(), 1, data));
        };
    }
}