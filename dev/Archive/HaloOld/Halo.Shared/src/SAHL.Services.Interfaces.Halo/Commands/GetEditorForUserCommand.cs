using SAHL.Core.Services;
using SAHL.Core.UI.Context;
using SAHL.Core.UI.Elements.Editors;
using System;

namespace SAHL.Services.Interfaces.Halo.Commands
{
    public class GetEditorForUserCommand : ServiceCommand, IServiceCommandWithReturnedData<EditorElement>, IHaloServiceCommand
    {
        public GetEditorForUserCommand(string userName, TileBusinessContext tileBusinessContext)
        {
            this.UserName = userName;
            this.TileBusinessContext = tileBusinessContext;
        }

        public EditorElement Result { get; set; }

        public string UserName { get; set; }

        public TileBusinessContext TileBusinessContext { get; set; }
    }
}