using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Configuration.Editors;
using SAHL.Core.UI.Halo.Editors.LegalEntity.CreateHelpDeskCaseEditor.Pages;

namespace SAHL.Config.Services.Halo.Modules.Client.Editors.LegalEntity
{
    public class CreateHelpDeskCaseEditorPage1Configuration : EditorOrderedPageConfiguration<CreateHelpDeskCaseEditorPage1Model>,
                                                              IParentedEditorPageConfiguration<CreateHelpDeskCaseEditorConfiguration>
    {
        public CreateHelpDeskCaseEditorPage1Configuration()
            : base(1)
        {
        }
    }
}