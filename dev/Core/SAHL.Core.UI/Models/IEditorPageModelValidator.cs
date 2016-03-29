using System.Collections.Generic;

namespace SAHL.Core.UI.Models
{
    public interface IEditorPageModelValidator
    {
        IEnumerable<IUIValidationResult> Validate(IEditorPageModel editorPageModel);
    }

    public interface IEditorPageModelValidator<T> : IEditorPageModelValidator where T : IEditorPageModel
    {
    }
}