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
    public class when_assigning_a_user_to_an_organisational_level : WithFakes
    {
        private static AssignUserInOrganisationStructureCommand command;
        private static AssignUserInOrganisationStructureCommandHandler handler;
        private static FakeDbFactory dbFactory;
        private static UserOrganisationStructureDataManager manager;
        private static IUnitOfWorkFactory unitOfWorkFactory;

        
        private Establish context = () =>
        {
            command = new AssignUserInOrganisationStructureCommand(1, 1);
            dbFactory = new FakeDbFactory();
            unitOfWorkFactory = An<IUnitOfWorkFactory>();
            manager = An<UserOrganisationStructureDataManager>(dbFactory, unitOfWorkFactory);
            handler = An<AssignUserInOrganisationStructureCommandHandler>(manager);
        };

        private Because of = () =>
        {
            handler.HandleCommand(command, new ServiceRequestMetadata());
        };

        private It should_call_to_the_manager= () =>
        {
            manager.WasToldTo(x => x.AddLinkOfAdUserToOrgStructure(1, 1));
        };

        private It should_call_to_the_database_with_the_insert_query = () =>
        {
            dbFactory.FakedDb.InAppContext().WasToldTo(x => x.Insert<Core.Data.Models._2AM.UserOrganisationStructureDataModel>(Arg.Any<InsertNewUserIntoStructureStatement>()));
        };
    }
}