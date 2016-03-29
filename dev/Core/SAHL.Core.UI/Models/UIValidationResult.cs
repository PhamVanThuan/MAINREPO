using SAHL.Core.UI.Enums;

namespace SAHL.Core.UI.Models
{
    public class UIValidationResult : IUIValidationResult
    {
        public UIValidationResult(ValidationSeverityLevel severity, string propertyName, string message)
        {
            this.Severity = severity;
            this.PropertyName = propertyName;
            this.Message = message;
        }

        public Enums.ValidationSeverityLevel Severity { get; protected set; }

        public string PropertyName { get; protected set; }

        public string Message { get; protected set; }
    }
}