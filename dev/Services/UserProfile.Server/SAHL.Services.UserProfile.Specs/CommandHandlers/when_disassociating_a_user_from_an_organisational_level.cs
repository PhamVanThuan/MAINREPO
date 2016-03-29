using System;
using System.Data;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.Interfaces.UserProfile.Commands;
using SAHL.Services.UserProfile.CommandHandlers;
using SAHL.Services.UserProfile.Managers;
using SAHL.Services.UserProfile.Managers.Statements;

namespace SAHL.Services.UserProfile.Specs.CommandHandlers
{
    public class when_disassociating_a_user_from_an_organisational_level : WithFakes
    {
        private static DissociateUserFromOrganisationStructureCommand command;
        private static DisassociateUserFromOrganisationStructureCommandHandler handler;
        private static FakeDbFactory dbFactory;
        private static UserOrganisationStructureDataManager manager;
        private static IUnitOfWorkFactory unitOfWorkFactory;
        private Establish context = () =>
        {
            command = new DissociateUserFromOrganisationStructureCommand(1, 1);
            dbFactory = new FakeDbFactory();
            unitOfWorkFactory = An<IUnitOfWorkFactory>();
            manager = An<UserOrganisationStructureDataManager>(dbFactory, unitOfWorkFactory);
            handler = An<DisassociateUserFromOrganisationStructureCommandHandler>(manager);
        };


        private Because of = () =>
        {
            handler.HandleCommand(command, new ServiceRequestMetadata());
        };

        private It should_call_to_the_manager = () =>
        {
            manager.WasToldTo(x => x.RemoveLinkOfAdUserToOrgStructure(1, 1));
        };

        private It should_call_to_the_database_with_the_remove_query = () =>
        {
            dbFactory.FakedDb.InAppContext().WasToldTo(x => x.Delete(Arg.Any<RemoveLinkFromUserToOrganisationStructureNodeStatement>()));
        };


    }
}
