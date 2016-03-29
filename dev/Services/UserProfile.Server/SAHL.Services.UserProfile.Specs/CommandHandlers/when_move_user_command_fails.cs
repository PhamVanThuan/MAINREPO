using System;
using System.Data.SqlClient;
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

namespace SAHL.Services.UserProfile.Specs.CommandHandlers
{
    public class when_move_user_command_fails : WithFakes
    {
        private static MoveUserInOrganisationStructureCommand command;
        private static MoveUserInOrganisationStructureCommandHandler handler;
        private static FakeDbFactory dbFactory;
        private static UserOrganisationStructureDataManager manager;
        private static IUnitOfWorkFactory unitOfWorkFactory;
        private static int toOrganisationStructureKey;
        private static int fromOrganisationStructureKey;
        private static int adUserKey;
        private static Exception exception;

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
            dbFactory.FakedDb.InAppContext().WhenToldTo(x => x.Insert<UserOrganisationStructureDataModel>(Arg.Any<InsertNewUserIntoStructureStatement>())).Throw(new Exception());
        };

        private Because of = () =>
        {
            exception = Catch.Exception(() =>handler.HandleCommand(command, new ServiceRequestMetadata()));
        };

        private It should_call_to_the_manager_to_remove_the_user_from_the_correct_org_structure_node = () =>
        {
            manager.WasToldTo(x => x.RemoveLinkOfAdUserToOrgStructure(adUserKey, fromOrganisationStructureKey));
        };

        private It should_call_to_the_database_with_the_remove_query = () =>
        {
            dbFactory.FakedDb.InAppContext().WasToldTo(x => x.Delete(Arg.Any<RemoveLinkFromUserToOrganisationStructureNodeStatement>()));
        };

        private It should_call_to_the_database_with_the_insert_statement = () =>
        {
            dbFactory.FakedDb.InAppContext().WasToldTo(x => x.Insert<UserOrganisationStructureDataModel>(Arg.Any<InsertNewUserIntoStructureStatement>()));
        };

        private It should_not_complete_the_unit_of_work = () =>
        {
            unitOfWorkFactory.Build().WasNotToldTo(x => x.Complete());
        };
    }
}