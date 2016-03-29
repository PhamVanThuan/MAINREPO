using SAHL.Core.UI.Enums;

namespace SAHL.Core.UI.Models
{
    public interface IUIValidationResult
    {
        ValidationSeverityLevel Severity { get; }

        string PropertyName { get; }

        string Message { get; }
    }
}