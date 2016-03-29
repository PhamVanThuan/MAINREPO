using System.ComponentModel.DataAnnotations;

namespace SAHL.Core.Tests.Validation.Fakes
{
    public class FakeValidationCommandCompositeGeneric<T>
    {
        [Required]
        public T GenericItem { get; set; }
    }
}