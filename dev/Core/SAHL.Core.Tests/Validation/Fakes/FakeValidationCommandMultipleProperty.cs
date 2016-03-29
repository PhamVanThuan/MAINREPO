using System.ComponentModel.DataAnnotations;

namespace SAHL.Core.Tests.Validation.Fakes
{
    public class FakeValidationCommandMultipleProperty
    {
        [Required]
        public string Key { get; set; }

        [Required]
        public string Id { get; set; }
    }
}