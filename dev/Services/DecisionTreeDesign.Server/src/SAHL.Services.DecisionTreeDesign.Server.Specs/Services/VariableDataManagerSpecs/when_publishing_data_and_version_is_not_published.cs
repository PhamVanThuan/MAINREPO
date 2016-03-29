using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.DecisionTree;
using SAHL.Services.DecisionTreeDesign.Managers.Variable;
using System;

namespace SAHL.Services.DecisionTreeDesign.Server.Specs.Services.VariableDataManagerSpecs
{
    [Subject("SAHL.Services.DecisionTree.Services.VariableDataManager.SaveAndPublishVariableSet")]
    public class when_publishing_data_and_version_is_not_published : WithFakes
    {
        private static IVariableManager variableManager;
        private static IVariableDataManager variableService;
        private static IUnitOfWorkFactory unitOfWorkFactory;

        private static Guid id;
        private static int version;
        private static string data;
        private static string publisher;
        private static Guid statusID;

        private Establish context = () =>
            {
                id = Guid.Empty;
                version = 0;
                data = "variables:[]";
                publisher = "Test";
                variableService = An<IVariableDataManager>();
                unitOfWorkFactory = An<IUnitOfWorkFactory>();
                variableService.WhenToldTo(x => x.IsVariableSetVersionPublished(version)).Return(false);
                variableService.WhenToldTo(x => x.DoesVariableSetExist(id)).Return(true);
                variableManager = new VariableManager(variableService, unitOfWorkFactory);
                statusID = Guid.Parse(PublishStatusEnumDataModel.IN_PROGRESS);
            };

        private Because of = () =>
            {
                variableManager.SaveAndPublishVariableSet(id, version, data, publisher);
            };

        private It should_check_if_current_version_is_published = () =>
            {
                variableService.WasToldTo(x => x.IsVariableSetVersionPublished(version));
            };

        private It should_check_if_current_id_exists = () =>
        {
            variableService.WasToldTo(x => x.DoesVariableSetExist(id));
        };

        private It should_update_the_current_version_of_the_variable_set = () =>
            {
                variableService.WasToldTo(x => x.UpdateVariableSet(id, version, data));
            };

        private It should_save_a_new_published_variable_set = () =>
        {
            variableService.WasToldTo(x => x.InsertPublishedVariableSet(Param.IsAny<Guid>(), Param.Is<Guid>(id), statusID, Param.IsAny<DateTime>(), publisher));
        };
    }
}