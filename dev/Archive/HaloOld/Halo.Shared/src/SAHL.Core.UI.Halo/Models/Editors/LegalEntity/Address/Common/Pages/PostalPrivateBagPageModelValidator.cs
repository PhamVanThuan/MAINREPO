using FluentValidation;
using SAHL.Core.UI.Halo.Editors.Address.Common.Pages;
using SAHL.Core.UI.Models;

namespace SAHL.Core.UI.Halo.Editors.LegalEntity.Address.Common.Pages
{
    public class PostalPrivateBagPageModelValidator : AbstractEditorPageModelValidator<PostalPrivateBagPageModel>
    {
        public PostalPrivateBagPageModelValidator()
        {
            RuleFor(x => x.PrivateBag).NotEmpty();
            RuleFor(x => x.PostOffice).NotEmpty();
        }
    }
}