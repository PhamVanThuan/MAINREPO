using System.ComponentModel.DataAnnotations;

namespace SAHL.Core.Tests.Validation.Fakes
{
    public class FakeValidationCommandUniqueValidationProperties
    {
        [Required]
        public string RequiredProperty { get; set; }

        [Range(1, 10)]
        public int RangeProperty { get; set; }

        [StringLength(10)]
        public string StringLengthProperty { get; set; }
    }
}