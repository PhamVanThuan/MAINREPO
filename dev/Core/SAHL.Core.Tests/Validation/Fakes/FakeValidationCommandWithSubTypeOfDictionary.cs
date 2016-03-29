using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Core.Tests.Validation.Fakes
{
    public class FakeValidationCommandWithSubTypeOfDictionary
    {
        [Required]
        public string Key { get; set; }

        [Required]
        public IDictionary<string, string> MetaData { get; set; }
    }

    public class ObjectWhichInheritsFromDictionaryWithRequiredAttribute : Dictionary<string, string>, IObjectWhichInheritsFromDictionary
    {
        [Required]
        public string Value1
        {
            get
            {
                return this.ContainsKey("Value1") ? this["Value1"] : "";
            }
        }

        public string Value2
        {
            get
            {
                return this.ContainsKey("Value2") ? this["Value2"] : "";
            }
        }
    }

    public class ObjectWhichInheritsFromDictionary : Dictionary<string, string>, IObjectWhichInheritsFromDictionary
    {
        public string Value1
        {
            get
            {
                return this.ContainsKey("Value1") ? this["Value1"] : "";
            }
        }

        public string Value2
        {
            get
            {
                return this.ContainsKey("Value2") ? this["Value2"] : "";
            }
        }
    }

    public interface IObjectWhichInheritsFromDictionary : IDictionary<string, string>
    {
        string Value1 { get; }
        string Value2 { get; }
    }
}
