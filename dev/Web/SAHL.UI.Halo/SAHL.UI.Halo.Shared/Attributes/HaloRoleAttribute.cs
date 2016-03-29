using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SAHL.UI.Halo.Shared
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class HaloRoleAttribute : Attribute
    {
        public HaloRoleAttribute(string roleName, params string[] capabilities)
        {
            if (string.IsNullOrWhiteSpace(roleName)) { throw new ArgumentNullException("roleName"); }

            this.RoleName     = roleName;
            this.Capabilities = capabilities;
        }

        public string RoleName { get; private set; }
        public string[] Capabilities { get; private set; }
    }
}
