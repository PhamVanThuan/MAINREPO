using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.UI.UserState.Managers;
using SAHL.Services.Interfaces.Halo.Commands;
using System;
using System.Security.Principal;

namespace SAHL.Services.Halo.CommandHandlers
{
    public class GetEditorPageContentForUserCommandHandler : IServiceCommandHandler<GetEditorPageContentForUserCommand>
    {
        private IUserStateManager userStateManager;

        public GetEditorPageContentForUserCommandHandler(IUserStateManager userStateManager)
        {
            this.userStateManager = userStateManager;
        }

        public ISystemMessageCollection HandleCommand(GetEditorPageContentForUserCommand command)
        {
            var editorPageContent = userStateManager.GetEditorPageContentForUser(new GenericPrincipal(new GenericIdentity(command.UserName), new string[] { }), command.EditBusinessContext);

            command.Result = editorPageContent;
            return new SystemMessageCollection();
        }
    }
}