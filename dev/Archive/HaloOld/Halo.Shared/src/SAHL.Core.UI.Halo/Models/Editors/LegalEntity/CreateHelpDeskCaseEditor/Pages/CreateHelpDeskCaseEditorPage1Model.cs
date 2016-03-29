using System;
using SAHL.Core.UI.Models;

namespace SAHL.Core.UI.Halo.Editors.LegalEntity.CreateHelpDeskCaseEditor.Pages
{
    public class CreateHelpDeskCaseEditorPage1Model : IEditorPageModel
    {
        public string NameOfRequestor { get; set; }

        public DateTime DateOfRequest { get; set; }

        public string Message { get; set; }

        public void Initialise(BusinessModel.BusinessContext businessContext)
        {
        }
    }
}