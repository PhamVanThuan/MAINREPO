using FluentValidation;
using FluentValidation.Results;
using System.Collections.Generic;

namespace SAHL.Core.UI.Models
{
    public class AbstractEditorPageModelValidator<T> : AbstractValidator<T>, IEditorPageModelValidator<T>
        where T : IEditorPageModel
    {
        public IEnumerable<IUIValidationResult> Validate(IEditorPageModel editorPageModel)
        {
            List<UIValidationResult> validationResults = new List<UIValidationResult>();
            ValidationResult validationResult = base.Validate((T)editorPageModel);
            foreach (var error in validationResult.Errors)
            {
                validationResults.Add(new UIValidationResult(Enums.ValidationSeverityLevel.Error, error.PropertyName, error.ErrorMessage));
            }
            return validationResults;
        }
    }
}