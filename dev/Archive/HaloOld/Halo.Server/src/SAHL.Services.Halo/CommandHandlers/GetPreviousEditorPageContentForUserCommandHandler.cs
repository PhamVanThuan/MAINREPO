using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.UI.UserState.Managers;
using SAHL.Services.Interfaces.Halo.Commands;
using System;
using System.Security.Principal;

namespace SAHL.Services.Halo.CommandHandlers
{
    public class GetPreviousEditorPageContentForUserCommandHandler : IServiceCommandHandler<GetPreviousEditorPageContentForUserCommand>
    {
        private IUserStateManager userStateManager;

        public GetPreviousEditorPageContentForUserCommandHandler(IUserStateManager userStateManager)
        {
            this.userStateManager = userStateManager;
        }

        public ISystemMessageCollection HandleCommand(GetPreviousEditorPageContentForUserCommand command)
        {
            var editorPageContent = userStateManager.GetPreviousEditorPageContentForUser(new GenericPrincipal(new GenericIdentity(command.UserName), new string[] { }), command.EditorBusinessContext);

            command.Result = editorPageContent;
            return new SystemMessageCollection();
        }
    }
}