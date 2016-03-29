using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.CommandHandlers;
using SAHL.Services.Capitec.Managers.Security;

using SAHL.Services.Interfaces.Capitec.Commands;
using System;
using System.Collections.Generic;
using SAHL.Core.Services;

namespace SAHL.Services.Capitec.Specs.CommandHandlerSpecs.ChangeBranchDetailsCommandHandlerSpecs
{
    public class when_told_to_change_branch_details : WithFakes
    {
        static ChangeBranchDetailsCommand command;
        static ChangeBranchDetailsCommandHandler handler;
        static ISecurityManager securityManager;
        static string branchName;
        static bool isActive;
        static Guid id, suburbId;
        static ISystemMessageCollection messages;
        static ServiceRequestMetadata metadata;

        Establish context = () =>
        {
            branchName = "New Branch";
            isActive = true;
            id = Guid.NewGuid();
            suburbId = Guid.NewGuid();
            securityManager = An<ISecurityManager>();
            command = new ChangeBranchDetailsCommand(id, branchName, isActive, suburbId);
            handler = new ChangeBranchDetailsCommandHandler(securityManager);
        };

        Because of = () =>
        {
            messages = handler.HandleCommand(command,metadata);
        };

        It should_change_the_branch_details_using_the_security_manager = () =>
        {
            securityManager.WasToldTo(x => x.ChangeBrancheDetails(Param.Is(command.Id), Param.Is(command.BranchName), Param.Is(command.IsActive), Param.Is(command.SuburbId)));
        };

        It should_not_return_any_error_messages = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}