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
    public class when_told_to_add_a_new_branch : WithFakes
    {
        static AddNewBranchCommand command;
        static AddNewBranchCommandHandler handler;
        static ISecurityManager securityManager;
        static string branchName;
        static string branchCode;
        static bool isActive;
        static Guid suburbId;
        static ISystemMessageCollection messages;
        static ServiceRequestMetadata metadata;

        Establish context = () =>
        {
            branchName = "New Branch";
            branchCode = "1234";
            isActive = true;
            suburbId = Guid.NewGuid();
            securityManager = An<ISecurityManager>();
            command = new AddNewBranchCommand(branchName, isActive, suburbId, branchCode);
            handler = new AddNewBranchCommandHandler(securityManager);
        };

        Because of = () =>
        {
            messages = handler.HandleCommand(command,metadata);
        };

        It should_add_the_branch_using_the_security_manager = () =>
        {
            securityManager.WasToldTo(x => x.AddBranch(Param.Is(command.BranchName), Param.Is(command.IsActive), Param.Is(command.SuburbId), Param.Is(branchCode)));
        };

        It should_not_return_any_error_messages = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}