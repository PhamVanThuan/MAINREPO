using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.UI.UserState.Managers;
using SAHL.Services.Interfaces.Halo.Commands;
using System;
using System.Security.Principal;

namespace SAHL.Services.Halo.CommandHandlers
{
    public class SubmitEditorForUserCommandHandler : IServiceCommandHandler<SubmitEditorForUserCommand>
    {
        private IUserStateManager userStateManager;

        public SubmitEditorForUserCommandHandler(IUserStateManager userStateManager)
        {
            this.userStateManager = userStateManager;
        }

        public ISystemMessageCollection HandleCommand(SubmitEditorForUserCommand command)
        {
            var editorPageContent = userStateManager.SubmitEditorForUser(new GenericPrincipal(new GenericIdentity(command.UserName), new string[] { }), command.EditorBusinessContext, command.FinalPageModel);

            command.Result = editorPageContent;
            return new SystemMessageCollection();
        }
    }
}