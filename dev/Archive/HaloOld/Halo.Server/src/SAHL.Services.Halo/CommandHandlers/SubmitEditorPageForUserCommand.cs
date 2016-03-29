using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.UI.UserState.Managers;
using SAHL.Services.Interfaces.Halo.Commands;
using System;
using System.Security.Principal;

namespace SAHL.Services.Halo.CommandHandlers
{
    public class SubmitEditorPageForUserCommandHandler : IServiceCommandHandler<SubmitEditorPageForUserCommand>
    {
        private IUserStateManager userStateManager;

        public SubmitEditorPageForUserCommandHandler(IUserStateManager userStateManager)
        {
            this.userStateManager = userStateManager;
        }

        public ISystemMessageCollection HandleCommand(SubmitEditorPageForUserCommand command)
        {
            var editorPageContent = userStateManager.SubmitEditorPageForUser(new GenericPrincipal(new GenericIdentity(command.UserName), new string[] { }), command.EditorBusinessContext, command.PageModelToValidate);

            command.Result = editorPageContent;
            return new SystemMessageCollection();
        }
    }
}