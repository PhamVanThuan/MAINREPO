using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.ObjectFromJsonGenerator.Lib.Templates
{
    public class EnumGroupDescription
    {
        public EnumGroupDescription(string groupName)
        {
            this.GroupName = groupName;
            this.Enums = new List<EnumDescription>();
        }
        public string GroupName { get; protected set; }
        public List<EnumDescription> Enums { get; protected set; }
        public List<EnumGroupDescription> subGroupDescriptions;
    }
}
