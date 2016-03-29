using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.Interfaces.UserProfile.Commands;
using SAHL.Services.UserProfile.CommandHandlers;
using SAHL.Services.UserProfile.Managers;

namespace SAHL.Services.UserProfile.Specs.CommandHandlers
{
    public class when_adding_an_organisational_structure_node_to_another : WithFakes
    {
        private static AddNewLevelToTheOrganisationStructureCommandHandler handler;
        private static IOrganisationStructreDataMangager manager;
        private static AddNewLevelToTheOrganisationStructureCommand command;
        private static FakeDbFactory dbFactory = new FakeDbFactory();
        private static int parentKey = 1;
        private static int organisationTypeKey = 10;
        private static string description = "";

        private Establish context = () =>
        {
            manager = An<OrganisationStructureDataMangager>(dbFactory);
            command = new AddNewLevelToTheOrganisationStructureCommand(parentKey, description, organisationTypeKey);
            handler = new AddNewLevelToTheOrganisationStructureCommandHandler(manager);
        };

        private Because of = () =>
        {
            handler.HandleCommand(command, new ServiceRequestMetadata());
        };

        private It should_call_the_data_manager = () =>
        {
            manager.WasToldTo(x => x.AddNewLevelAtPoint(parentKey, organisationTypeKey, description));
        };

        private It should_insert_new_item_into_database = () =>
        {
            dbFactory.FakedDb.InAppContext().WasToldTo(x => x.Insert(Arg.Any<OrganisationStructureDataModel>()));
        };
    }
}