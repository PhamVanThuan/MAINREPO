using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Core.Validation
{
    public interface IValidateCommand
    {
        IEnumerable<ValidationResult> Validate(object objectToValidate);
    }
}