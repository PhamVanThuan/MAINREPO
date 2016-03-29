using SAHL.Core.Services;
using SAHL.Core.UI.Context;
using SAHL.Core.UI.Models;
using System;

namespace SAHL.Services.Interfaces.Halo.Commands
{
    public class GetPreviousEditorPageContentForUserCommand : ServiceCommand, IServiceCommandWithReturnedData<IUIEditorPageModel>, IHaloServiceCommand
    {
        public GetPreviousEditorPageContentForUserCommand(string userName, EditorBusinessContext editorBusinessContext)
        {
            this.UserName = userName;
            this.EditorBusinessContext = editorBusinessContext;
        }

        public IUIEditorPageModel Result { get; set; }

        public EditorBusinessContext EditorBusinessContext { get; set; }

        public string UserName { get; set; }
    }
}