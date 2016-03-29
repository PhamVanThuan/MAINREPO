using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.UserProfile.Models.Shared
{
    public class UserAreaRole
    {
        public string OrganisationArea { get; protected set; }
        public string RoleName { get; protected set; }
        public int? UserOrganisationstructureKey { get; set; }

        public UserAreaRole(string organisationArea, string roleName, int? userOrganisationStructureKey)
        {
            this.OrganisationArea = organisationArea;
            this.RoleName = roleName;
            this.UserOrganisationstructureKey = userOrganisationStructureKey;
        }
    }
}
