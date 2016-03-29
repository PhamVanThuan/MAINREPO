using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Core.Tests.Validation.Fakes
{
    public class FakeValidationCommandCompositeList
    {
        [Required]
        public string Key { get; set; }

        [Required]
        public string Id { get; set; }

        public List<FakeValidationCommandSingleProperty> List { get; set; }
    }
}