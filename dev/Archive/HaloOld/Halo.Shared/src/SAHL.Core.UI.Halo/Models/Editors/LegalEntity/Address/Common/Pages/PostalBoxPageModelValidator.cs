using FluentValidation;
using SAHL.Core.UI.Halo.Editors.Address.Common.Pages;
using SAHL.Core.UI.Models;

namespace SAHL.Core.UI.Halo.Editors.LegalEntity.Address.Common.Pages
{
    public class PostalBoxPageModelValidator : AbstractEditorPageModelValidator<PostalBoxPageModel>
    {
        public PostalBoxPageModelValidator()
        {
            RuleFor(x => x.BoxNumber).NotEmpty();
            RuleFor(x => x.PostOffice).NotEmpty();
        }
    }
}