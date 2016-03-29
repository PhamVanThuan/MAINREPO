using FluentValidation;
using SAHL.Core.UI.Halo.Editors.Address.Common.Pages;
using SAHL.Core.UI.Models;

namespace SAHL.Core.UI.Halo.Editors.LegalEntity.Address.Common.Pages
{
    public class PostalStreetPageModelValidator : AbstractEditorPageModelValidator<PostalStreetPageModel>
    {
        public PostalStreetPageModelValidator()
        {
            RuleFor(x => x.SelectedProvinceKey).NotEmpty().WithMessage("A Province must be selected");
            RuleFor(x => x.Suburb).NotEmpty();
        }
    }
}