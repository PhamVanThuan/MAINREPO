using FluentValidation;
using SAHL.Core.UI.Models;

namespace SAHL.Core.UI.Halo.Editors.LegalEntity.CreateHelpDeskCaseEditor.Pages
{
    public class CreateHelpDeskCaseEditorPage1ModelValidator : AbstractEditorPageModelValidator<CreateHelpDeskCaseEditorPage1Model>
    {
        public CreateHelpDeskCaseEditorPage1ModelValidator()
        {
            RuleFor(model => model.NameOfRequestor).NotEmpty();
        }
    }
}