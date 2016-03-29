using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Services.DecisionTreeDesign.Managers.EnumerationSet;
using System;

namespace SAHL.Services.DecisionTreeDesign.Server.Specs.Services.EnumerationSetManagerSpecs
{
    [Subject("SAHL.Services.DecisionTree.Services.EnumerationSetManager.SaveAndPublishEnumerationSet")]
    public class when_publishing_data_when_not_published : WithFakes
    {
        private static IEnumerationSetManager enumManager;
        private static IEnumerationSetDataManager enumDataService;
        private static IUnitOfWorkFactory unitOfWorkFactory;

        private static Guid id;
        private static int version;
        private static string data;
        private static string publisher;

        private Establish context = () =>
        {
            id = Guid.Empty;
            version = 0;

            enumDataService = An<IEnumerationSetDataManager>();
            unitOfWorkFactory = An<IUnitOfWorkFactory>();
            enumDataService.WhenToldTo(x => x.DoesEnumerationSetExist(id)).Return(true);
            enumManager = new EnumerationSetManager(enumDataService, unitOfWorkFactory);
        };

        private Because of = () =>
        {
            enumManager.SaveAndPublishEnumerationSet(id, version, data, publisher);
        };

        private It should_determine_if_current_version_is_published = () =>
        {
            enumDataService.WasToldTo(x => x.IsEnumerationSetVersionPublished(version));
        };

        private It should_determine_if_current_id_exists = () =>
        {
            enumDataService.WasToldTo(x => x.DoesEnumerationSetExist(id));
        };

        private It should_save_enumeration_set = () =>
        {
            enumDataService.WasToldTo(x => x.UpdateEnumerationSet(id, version, data));
        };

        private It should_publish_enumeration_set = () =>
        {
            enumDataService.WasToldTo(x => x.InsertPublishedEnumerationSet(Param.IsAny<Guid>(), Param.IsAny<Guid>(), Param.IsAny<Guid>(), Param.IsAny<DateTime>(), Param.IsAny<string>()));
        };
    }
}