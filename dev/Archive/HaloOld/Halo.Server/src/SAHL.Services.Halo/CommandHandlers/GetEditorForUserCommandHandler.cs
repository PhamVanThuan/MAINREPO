using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.UI.UserState.Managers;
using SAHL.Services.Interfaces.Halo.Commands;
using System;
using System.Security.Principal;

namespace SAHL.Services.Halo.CommandHandlers
{
    public class GetEditorForUserCommandHandler : IServiceCommandHandler<GetEditorForUserCommand>
    {
        private IUserStateManager userStateManager;

        public GetEditorForUserCommandHandler(IUserStateManager userStateManager)
        {
            this.userStateManager = userStateManager;
        }

        public ISystemMessageCollection HandleCommand(GetEditorForUserCommand command)
        {
            var editorElement = userStateManager.ShowEditorForUser(new GenericPrincipal(new GenericIdentity(command.UserName), new string[] { }), command.TileBusinessContext);
            command.Result = editorElement;
            return new SystemMessageCollection();
        }
    }
}