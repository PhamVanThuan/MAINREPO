using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.CommandHandlers;
using SAHL.Services.Capitec.Managers.Security;

using SAHL.Services.Interfaces.Capitec.Commands;
using System;
using System.Collections.Generic;
using SAHL.Core.Services;

namespace SAHL.Services.Capitec.Specs.CommandHandlerSpecs.AddNewBranchCommandHandlerSpecs
{
    public class when_told_to_add_a_new_user : WithFakes
    {
        static AddNewUserCommand command;
        static AddNewUserCommandHandler handler;
        static ISecurityManager securityManager;
        static string username, emailAddress, firstName, lastName, rolesToAssign;
        static bool isActive;
        static Guid branchId;
        static ISystemMessageCollection messages;
        static ServiceRequestMetadata metadata;

        Establish context = () =>
        {
            username = "ClintonS";
            emailAddress = "clintons@sahomeloans.com";
            firstName = "Clint";
            lastName = "Speed";
            rolesToAssign = Guid.NewGuid().ToString();
            branchId = Guid.NewGuid();
            securityManager = An<ISecurityManager>();
            command = new AddNewUserCommand(username, emailAddress, firstName, lastName, rolesToAssign, branchId);
            handler = new AddNewUserCommandHandler(securityManager);
        };

        Because of = () =>
        {
            messages = handler.HandleCommand(command,metadata);
        };

        It should_add_the_branch_using_the_security_manager = () =>
        {
            securityManager.WasToldTo(x => x.AddUser(Param.Is(command.Username), Param.Is(command.EmailAddress), Param.Is(command.FirstName), Param.Is(command.LastName), 
                Param.IsAny<Guid[]>(), Param.Is(command.BranchId)));
        };

        It should_not_return_any_error_messages = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}