using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Services.DecisionTreeDesign.Managers.Variable;
using System;

namespace SAHL.Services.DecisionTreeDesign.Server.Specs.Services.VariableDataManagerSpecs
{
    [Subject("SAHL.Services.DecisionTree.Services.VariableDataManager.SaveVariableSet")]
    public class when_saving_data_and_version_not_published : WithFakes
    {
        private static IVariableManager variableManager;
        private static IVariableDataManager variableService;
        private static IUnitOfWorkFactory unitOfWorkFactory;

        private static Guid id;
        private static int version;
        private static string data;

        private Establish context = () =>
            {
                id = Guid.Empty;
                version = 0;
                data = "variables:[]";
                variableService = An<IVariableDataManager>();
                unitOfWorkFactory = An<IUnitOfWorkFactory>();
                variableManager = new VariableManager(variableService, unitOfWorkFactory);
                variableService.WhenToldTo(x => x.IsVariableSetVersionPublished(version)).Return(false);
                variableService.WhenToldTo(x => x.DoesVariableSetExist(id)).Return(true);
            };

        private Because of = () =>
            {
                variableManager.SaveVariableSet(id, version, data);
            };

        private It should_check_if_current_version_is_published = () =>
            {
                variableService.WasToldTo(x => x.IsVariableSetVersionPublished(version));
            };

        private It should_check_if_current_id_exists = () =>
        {
            variableService.WasToldTo(x => x.DoesVariableSetExist(id));
        };

        private It should_update_the_current_version_of_variable_set = () =>
            {
                variableService.WasToldTo(x => x.UpdateVariableSet(id, version, data));
            };
    }
}