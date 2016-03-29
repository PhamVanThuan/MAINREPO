using System.Collections.Generic;
using System.Linq;

namespace SAHL.Tools.EnumGenerator.Templates
{
    public partial class EnumTemplate
    {
        private List<string> enumValues;

        public EnumTemplate(string enumName, IEnumerable<string> enumValues)
        {
            this.EnumName = enumName;
            this.enumValues = new List<string>(enumValues.Select(x => x + ","));
            this.enumValues[this.enumValues.Count - 1] = this.enumValues[this.enumValues.Count - 1].Replace(",", "");
        }

        public string EnumName { get; protected set; }

        public IEnumerable<string> EnumValues { get { return this.enumValues; } }
    }
}