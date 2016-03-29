using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.DashboardViewGenerator.Templates
{
    public partial class DashboardJSTemplate
    {
        TypeDefinition _type;

        public string ShortPathName
        {
            get
            {
                return _type.FullName.StringAfterWord("Configuration.");
            }
        }

        public string RelativePath
        {
            get
            {
                return this.ShortPathName.Replace(".", "\\") + ".js";
            }
        }

        public DashboardJSTemplate(TypeDefinition type)
        {
            this._type = type;
        }
    }
}
