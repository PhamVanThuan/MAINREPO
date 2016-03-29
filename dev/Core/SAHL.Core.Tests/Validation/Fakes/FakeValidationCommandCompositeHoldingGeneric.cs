using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Core.Tests.Validation.Fakes
{
    public class FakeValidationCommandCompositeHoldingGeneric
    {
        [Required]
        public List<FakeValidationCommandCompositeGeneric<string>> GenericItems { get; set; }
    }
}