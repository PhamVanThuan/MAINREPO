using FluentValidation;
using SAHL.Core.UI.Halo.Editors.Address.Common.Pages;
using SAHL.Core.UI.Models;

namespace SAHL.Core.UI.Halo.Editors.LegalEntity.Address.Common.Pages
{
    public class ResidentialFreeTextPageModelValidator : AbstractEditorPageModelValidator<ResidentialFreeTextPageModel>
    {
        public ResidentialFreeTextPageModelValidator()
        {
            RuleFor(x => x.Line1).NotEmpty();
            RuleFor(x => x.SelectedCountryKey).NotEmpty().WithMessage("A Country must be selected");
        }
    }
}