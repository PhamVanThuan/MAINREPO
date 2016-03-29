using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Core.Tests.Validation.Fakes
{
    public class FakeValidationCommandCompositeListWithList
    {
        [Required]
        public string Key { get; set; }

        [Required]
        public string Id { get; set; }

        public List<FakeValidationCommandCompositeList> Composite { get; set; }

        public int IdN { get; set; }
    }
}