using FluentValidation;
using SAHL.Core.UI.Halo.Editors.Address.Common.Pages;
using SAHL.Core.UI.Models;

namespace SAHL.Core.UI.Halo.Editors.LegalEntity.Address.Common.Pages
{
    public class PostalPostNetSuitePageModelValidator : AbstractEditorPageModelValidator<PostalPostNetSuitePageModel>
    {
        public PostalPostNetSuitePageModelValidator()
        {
            RuleFor(x => x.PostnetSuiteNumber).NotEmpty();
            RuleFor(x => x.PrivateBag).NotEmpty();
            RuleFor(x => x.PostOffice).NotEmpty();
        }
    }
}