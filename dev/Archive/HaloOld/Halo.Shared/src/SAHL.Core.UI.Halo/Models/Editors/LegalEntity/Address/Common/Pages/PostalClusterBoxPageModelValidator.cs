using FluentValidation;
using SAHL.Core.UI.Halo.Editors.Address.Common.Pages;
using SAHL.Core.UI.Models;

namespace SAHL.Core.UI.Halo.Editors.LegalEntity.Address.Common.Pages
{
    public class PostalClusterBoxPageModelValidator : AbstractEditorPageModelValidator<PostalClusterBoxPageModel>
    {
        public PostalClusterBoxPageModelValidator()
        {
            RuleFor(x => x.ClusterBoxNumber).NotEmpty();
            RuleFor(x => x.PostOffice).NotEmpty();
        }
    }
}