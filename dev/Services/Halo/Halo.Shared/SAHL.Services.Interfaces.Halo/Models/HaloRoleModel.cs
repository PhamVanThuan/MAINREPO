using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SAHL.Services.Interfaces.Halo.Models
{
    public class HaloRoleModel
    {
        public HaloRoleModel(string organisationArea, string roleName, string[] capabilities)
        {
            this.OrganisationArea = organisationArea;
            this.RoleName         = roleName;
            this.Capabilities     = capabilities;
        }

        public string OrganisationArea { get; set; }
        public string RoleName { get; set; }
        public string[] Capabilities { get; set; }
    }
}
