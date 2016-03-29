using System.ComponentModel.DataAnnotations;

namespace SAHL.Core.Tests.Validation.Fakes
{
    public class FakeValidationCommandSingleProperty
    {
        [Required]
        public string Key { get; set; }
    }
}