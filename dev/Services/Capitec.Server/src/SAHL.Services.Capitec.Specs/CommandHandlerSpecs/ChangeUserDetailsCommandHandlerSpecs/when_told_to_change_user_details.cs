using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.CommandHandlers;
using SAHL.Services.Capitec.Managers.Security;

using SAHL.Services.Interfaces.Capitec.Commands;
using System;
using System.Collections.Generic;
using SAHL.Core.Services;

namespace SAHL.Services.Capitec.Specs.CommandHandlerSpecs.ChangeUserDetailsCommandHandlerSpecs
{
    public class when_told_to_change_user_details : WithFakes
    {
        static ChangeUserDetailsCommand command;
        static ChangeUserDetailsCommandHandler handler;
        static ISecurityManager securityManager;
        static string username, emailAddress, firstName, lastName, rolesToAssign, rolesToRemove;
        static bool status;
        static Guid id, branchId;
        static ISystemMessageCollection messages;
        static ServiceRequestMetadata metadata;

        Establish context = () =>
        {
            status = true;
            emailAddress = "clintons@sahomeloans.com";
            firstName = "Clint";
            lastName = "Speed";
            rolesToAssign = Guid.NewGuid().ToString();
            rolesToRemove = Guid.NewGuid().ToString();
            branchId = Guid.NewGuid();
            securityManager = An<ISecurityManager>();
            command = new ChangeUserDetailsCommand(id, emailAddress, firstName, lastName, status, rolesToAssign, rolesToRemove, branchId);
            handler = new ChangeUserDetailsCommandHandler(securityManager);
        };

        Because of = () =>
        {
            messages = handler.HandleCommand(command,metadata);
        };

        It should_add_the_branch_using_the_security_manager = () =>
        {
            securityManager.WasToldTo(x => x.ChangeUserDetails(Param.Is(command.Id), Param.Is(command.EmailAddress), Param.Is(command.FirstName), Param.Is(command.LastName),
                Param.Is(status), Param.IsAny<Guid[]>(), Param.IsAny<Guid[]>(), Param.Is(command.BranchId)));
        };

        It should_not_return_any_error_messages = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}
