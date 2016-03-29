using System;
using System.Data;
using System.IO.Pipes;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.Interfaces.UserProfile.Commands;
using SAHL.Services.UserProfile.CommandHandlers;
using SAHL.Services.UserProfile.Managers;
using SAHL.Services.UserProfile.Managers.Statements;
using SAHL.Services.UserProfile.Specs.Helpers;

namespace SAHL.Services.UserProfile.Specs.CommandHandlers
{
    public class when_moving_a_user_between_organisation_levels : WithFakes
    {
        private static MoveUserInOrganisationStructureCommand command;
        private static MoveUserInOrganisationStructureCommandHandler handler;
        private static FakeDbFactory dbFactory;
        private static UserOrganisationStructureDataManager manager;
        private static IUnitOfWorkFactory unitOfWorkFactory;
        private static int toOrganisationStructureKey;
        private static int fromOrganisationStructureKey;
        private static int adUserKey;

        private Establish context = () =>
        {
            toOrganisationStructureKey = 2;
            fromOrganisationStructureKey = 1;
            adUserKey = 1;
            command = new MoveUserInOrganisationStructureCommand(adUserKey, fromOrganisationStructureKey, toOrganisationStructureKey);
            dbFactory = new FakeDbFactory();
            unitOfWorkFactory = new FakeUnitOfWorkFactory();
            manager = An<UserOrganisationStructureDataManager>(dbFactory, unitOfWorkFactory);
            handler = An<MoveUserInOrganisationStructureCommandHandler>(manager);
        };

        private Because of = () =>
        {
            handler.HandleCommand(command, new ServiceRequestMetadata());
        };

        private It should_call_to_the_manager_to_remove_the_user_from_the_correct_org_structure_node = () =>
        {
            manager.WasToldTo(x => x.RemoveLinkOfAdUserToOrgStructure(adUserKey, fromOrganisationStructureKey));
        };

        private It should_call_to_the_database_with_the_remove_query = () =>
        {
            dbFactory.FakedDb.InAppContext().WasToldTo(x => x.Delete(Arg.Any<RemoveLinkFromUserToOrganisationStructureNodeStatement>()));
        };


        private It should_call_to_the_manager_to_insert_the_user_into_the_new_org_structure_node = () =>
        {
            manager.WasToldTo(x => x.AddLinkOfAdUserToOrgStructure(adUserKey, toOrganisationStructureKey));
        };

        private It should_call_to_the_database_with_the_insert_statement = () =>
        {
            dbFactory.FakedDb.InAppContext().WasToldTo(x => x.Insert<UserOrganisationStructureDataModel>(Arg.Any<InsertNewUserIntoStructureStatement>()));
        };

        private It should_complete_the_unit_of_work = () =>
        {
            unitOfWorkFactory.Build().WasToldTo(x => x.Complete());
        };



    }
}
