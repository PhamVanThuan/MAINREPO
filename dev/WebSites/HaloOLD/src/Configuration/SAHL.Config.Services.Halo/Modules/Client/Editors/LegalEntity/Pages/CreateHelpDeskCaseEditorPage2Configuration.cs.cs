using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Configuration.Editors;
using SAHL.Core.UI.Halo.Editors.LegalEntity.CreateHelpDeskCaseEditor.Pages;

namespace SAHL.Config.Services.Halo.Modules.Client.Editors.LegalEntity
{
    public class CreateHelpDeskCaseEditorPage2Configuration : EditorOrderedPageConfiguration<CreateHelpDeskCaseEditorPage2Model>,
                                                              IParentedEditorPageConfiguration<CreateHelpDeskCaseEditorConfiguration>
    {
        public CreateHelpDeskCaseEditorPage2Configuration()
            : base(2)
        {
        }
    }
}