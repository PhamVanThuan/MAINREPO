using SAHL.Config.Services.Halo.Modules.Client.Tiles.ClientDashboard.Actions;
using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Configuration.Editors;
using SAHL.Core.UI.Enums;
using SAHL.Core.UI.Halo.Editors.LegalEntity.CreateHelpDeskCaseEditor;

namespace SAHL.Config.Services.Halo.Modules.Client.Editors.LegalEntity.CreateHelpDeskCase
{
    public class CreateHelpDeskCaseEditorConfiguration : EditorConfiguration<CreateHelpDeskCaseEditor>,
                                                         IParentedEditorConfiguration<CreateHelpDeskCaseActionConfiguration>
    {
        public CreateHelpDeskCaseEditorConfiguration()
            : base("Create Help Desk Case", EditorAction.Add)
        {
        }
    }
}