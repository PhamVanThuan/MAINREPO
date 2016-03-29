using SAHL.Core.Services;
using SAHL.Core.UI.Context;
using SAHL.Core.UI.Models;
using System;

namespace SAHL.Services.Interfaces.Halo.Commands
{
    public class GetEditorPageContentForUserCommand : ServiceCommand, IServiceCommandWithReturnedData<IUIEditorPageModel>, IHaloServiceCommand
    {
        public GetEditorPageContentForUserCommand(string userName, EditorBusinessContext editorBusinessContext)
        {
            this.UserName = userName;
            this.EditBusinessContext = editorBusinessContext;
        }

        public IUIEditorPageModel Result { get; set; }

        public EditorBusinessContext EditBusinessContext { get; set; }

        public string UserName { get; set; }
    }
}