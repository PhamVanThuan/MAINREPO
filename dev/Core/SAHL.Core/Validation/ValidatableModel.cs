using System.ComponentModel.DataAnnotations;

namespace SAHL.Core.Validation
{
    public abstract class ValidatableModel
    {
        public void Validate()
        {
            var context = new ValidationContext(this, null, null);
            Validator.ValidateObject(this, context, true);
        }
    }
}