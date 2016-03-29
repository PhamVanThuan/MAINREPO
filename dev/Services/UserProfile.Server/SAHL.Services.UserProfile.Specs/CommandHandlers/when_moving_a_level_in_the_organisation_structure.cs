using System;
using System.Data;
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
    public class when_moving_a_level_in_the_organisation_structure : WithFakes
    {
        private static MoveLevelInOrganisationStructureCommandHandler handler;
        private static IOrganisationStructreDataMangager manager;
        private static MoveLevelInOrganisationStructureCommand command;
        private static FakeDbFactory dbFactory = new FakeDbFactory();
        private static int parentKey = 1;
        private static int organisationTypeKey = 10;
        private static string description = "";

        private Establish context = () =>
        {
            manager = An<OrganisationStructureDataMangager>(dbFactory);
            command =new MoveLevelInOrganisationStructureCommand(organisationTypeKey, parentKey);
            handler = new MoveLevelInOrganisationStructureCommandHandler(manager);
        };

        private Because of = () =>
        {
            handler.HandleCommand(command, new ServiceRequestMetadata());
        };

        private It should_call_the_data_manager_to_update_the_parentHierachy = () =>
        {
            manager.WasToldTo(x => x.MoveOrganisationStructurelevel(organisationTypeKey, parentKey));
        };

        private It should_call_the_database_to_update_the_record = () =>
        {
            dbFactory.FakedDb.InAppContext().WasToldTo(x =>x.Update<OrganisationStructureDataModel>(Arg.Any<MoveOrganisationLevelToDifferentParentStatement>()));
        };

    }
}
