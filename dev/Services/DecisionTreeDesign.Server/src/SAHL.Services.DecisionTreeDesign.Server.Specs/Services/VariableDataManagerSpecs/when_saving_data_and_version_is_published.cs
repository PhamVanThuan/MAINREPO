using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Services.DecisionTreeDesign.Managers.Variable;
using System;

namespace SAHL.Services.DecisionTreeDesign.Server.Specs.Services.VariableDataManagerSpecs
{
    [Subject("SAHL.Services.DecisionTree.Services.VariableDataManager.SaveVariableSet")]
    public class when_saving_data_and_version_is_published : WithFakes
    {
        private static IVariableManager variableManager;
        private static IVariableDataManager variableDataManager;
        private static IUnitOfWorkFactory unitOfWorkFactory;

        private static Guid id;
        private static int version;
        private static string data;

        private Establish context = () =>
            {
                id = Guid.Empty;
                version = 0;
                data = "variables:[]";
                variableDataManager = An<IVariableDataManager>();
                unitOfWorkFactory = An<IUnitOfWorkFactory>();
                variableManager = new VariableManager(variableDataManager, unitOfWorkFactory);
                variableDataManager.WhenToldTo(x => x.IsVariableSetVersionPublished(version)).Return(true);
            };

        private Because of = () =>
            {
                variableManager.SaveVariableSet(id, version, data);
            };

        private It should_check_if_current_version_is_published = () =>
            {
                variableDataManager.WasToldTo(x => x.IsVariableSetVersionPublished(version));
            };

        private It should_save_a_new_version_of_the_variable_set = () =>
            {
                variableDataManager.WasToldTo(x => x.SaveVariableSet(Param.IsAny<Guid>(), 1, data));
            };
    }
}