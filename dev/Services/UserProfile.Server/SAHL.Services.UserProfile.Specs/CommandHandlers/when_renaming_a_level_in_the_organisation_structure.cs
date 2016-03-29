using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.Interfaces.UserProfile.Commands;
using SAHL.Services.UserProfile.CommandHandlers;
using SAHL.Services.UserProfile.Managers;
using SAHL.Services.UserProfile.Managers.Statements;

namespace SAHL.Services.UserProfile.Specs.CommandHandlers
{
    public class when_renaming_a_level_in_the_organisation_structure : WithFakes
    {
        private static RenameLevelInOrganisationStructureCommandHandler handler;
        private static IOrganisationStructreDataMangager manager;
        private static RenameLevelInOrganisationStructureCommand command;
        private static FakeDbFactory dbFactory = new FakeDbFactory();
        private static int parentKey = 1;
        private static int organisationTypeKey = 10;
        private static string description = "";

        private Establish context = () =>
        {
            manager = An<OrganisationStructureDataMangager>(dbFactory);
            command = new RenameLevelInOrganisationStructureCommand(organisationTypeKey, description);
            handler = new RenameLevelInOrganisationStructureCommandHandler(manager);
        };

        private Because of = () =>
        {
            handler.HandleCommand(command, new ServiceRequestMetadata());
        };

        private It should_call_the_manager_to_update_the_description = () =>
        {
            manager.WasToldTo(x => x.RenameLevelInStructure(organisationTypeKey, description));
        };

        private It should_call_the_database_to_update = () =>
        {
            dbFactory.FakedDb.InAppContext().WasToldTo(x => x.Update<OrganisationStructureDataModel>(Arg.Any<RenameOrganisationStructureLevelStatement>()));
        };
    }
}