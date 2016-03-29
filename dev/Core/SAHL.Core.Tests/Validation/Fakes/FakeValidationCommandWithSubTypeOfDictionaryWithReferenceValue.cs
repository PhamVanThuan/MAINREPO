using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Core.Tests.Validation.Fakes
{
    public class FakeValidationCommandWithSubTypeOfDictionaryWithReferenceValue
    {
        [Required]
        public string Key { get; set; }

        [Required]
        public IDictionary<string, FakeValidationCommandSingleProperty> MetaData { get; set; }
    }

    public class ObjectWhichInheritsFromDictionaryWithReferenceValueWithRequiredAttribute : Dictionary<string, FakeValidationCommandSingleProperty>
    {
    }
}
